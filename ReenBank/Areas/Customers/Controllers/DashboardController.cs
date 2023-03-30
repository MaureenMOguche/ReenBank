using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using RB.DataAccess.Repository.IRepository;
using RB.Models;
using RB.Models.ViewModels;
using RB.Utility;
using System.Security.Claims;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ReenBank.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _db;
        private IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public ProfileAccountTransactionsVM accountVM { get; set; } = new();
        public DashboardController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            accountVM.SenderUser = _db.ApplicationUserRepo.GetOne(x => x.Id == claims.Value);
            accountVM.SenderAccount = _db.BankAccountRepo
                .GetOne(x => x.Id == accountVM.SenderUser.BankAccountId);
            accountVM.TransactionsList = _db.TransactionsRepo
                .GetAll().Where(x => x.BankAccountId == accountVM.SenderAccount.Id);


            foreach (var item in accountVM.TransactionsList)
            {
                if (item.TransactionType == TransactionType.Credit)
                {
                    accountVM.TotalIncome += item.Amount;
                }
                else
                {
                    accountVM.TotalExpenses += item.Amount;
                }
            }

            return View(accountVM);
        }


        [HttpGet]
        public IActionResult Accounts()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            accountVM.SenderUser = _db.ApplicationUserRepo.GetOne(x => x.Id == claims.Value);
            accountVM.SenderAccount = _db.BankAccountRepo
                .GetOne(x => x.Id == accountVM.SenderUser.BankAccountId);
            accountVM.TransactionsList = _db.TransactionsRepo
                .GetAll().Where(x => x.BankAccountId == accountVM.SenderAccount.Id);


            return View(accountVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(ProfileAccountTransactionsVM accountVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            accountVM.SenderUser = _db.ApplicationUserRepo
                .GetOne(x => x.Id == claims.Value);
            accountVM.SenderAccount = _db.BankAccountRepo
                .GetOne(x => x.Id == accountVM.SenderUser.BankAccountId);

            _db.BankAccountRepo.Deposit(accountVM.SenderAccount, accountVM.DepositAmount);
            _db.Save();
            return LocalRedirect("/Customers/Dashboard");
        }

        //[HttpGet]
        //public IActionResult SearchT(string searchAccount)
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    accountVM.SenderUser = _db.ApplicationUserRepo
        //        .GetOne(x => x.Id == claims.Value);
        //    accountVM.SenderAccount = _db.BankAccountRepo
        //        .GetOne(x => x.Id == accountVM.SenderUser.BankAccountId);

        //    //Search Functionality
        //    if (!String.IsNullOrEmpty(searchAccount))
        //    {
        //        accountVM.ReceiverAccount = _db.BankAccountRepo
        //            .GetOne(x => x.AccountNumber == searchAccount);
        //    }
        //    return View(accountVM);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(ProfileAccountTransactionsVM accountVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            accountVM.SenderUser = _db.ApplicationUserRepo
                .GetOne(x => x.Id == claims.Value);
            accountVM.SenderAccount = _db.BankAccountRepo
                .GetOne(x => x.Id == accountVM.SenderUser.BankAccountId);


            var transferStatus = _db.BankAccountRepo.Transfer(accountVM.SenderAccount, accountVM.ReceiverAccount, accountVM.DepositAmount);

            
            _db.Save();
            if (transferStatus == "Invalid Account" || transferStatus == "Insufficient")
            {
                TempData["TransferError"] = transferStatus;
            }
            else
            {
                TempData["TransferSuccess"] = transferStatus;
            }

            return LocalRedirect("/Customers/Dashboard");
        }

        public TransactionsVM transactions = new();

        public IActionResult Transactions(DateTime? StartDate, DateTime? EndDate)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            transactions.AccountUser = _db.ApplicationUserRepo.GetOne(x => x.Id == claims.Value);
            transactions.BankAccount = _db.BankAccountRepo
                .GetOne(x => x.Id == transactions.AccountUser.BankAccountId);
            
            IEnumerable<Transactions> transList = _db.TransactionsRepo
                .GetAll().Where(x => x.BankAccountId == transactions.BankAccount.Id);

            if (StartDate != null)
            {
                transList = transList.Where(x => x.TransactionDate >= StartDate);
            }
            else if (StartDate == null && EndDate != null)
            {
                transList = transList.Where(x => x.TransactionDate <= EndDate);
            }
            else if (StartDate != null && EndDate != null)
            {
                transList = transList.Where(x => x.TransactionDate >= StartDate && x.TransactionDate <= EndDate);
            }


            transactions.TransactionsList = transList;
                       
          
            return View(transactions);
        }


        

        public IActionResult Profile()
        {
            return View();
        }

        
        //Print Pdf Statement
        [HttpGet("printstatement")]
        public async Task<IActionResult> PrintStatement()
        {
            //Getting User Identity so as to extract Account Number
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            accountVM.SenderUser = _db.ApplicationUserRepo.GetOne(x => x.Id == claims.Value);
            accountVM.SenderAccount = _db.BankAccountRepo
                .GetOne(x => x.Id == accountVM.SenderUser.BankAccountId);
            accountVM.TransactionsList = _db.TransactionsRepo
                .GetAll().Where(x => x.BankAccountId == accountVM.SenderAccount.Id);


            //Generating Pdf
            var document = new PdfDocument();
            string htmlBody = "<h1 style=\"text-align:center;color:#0F855F\"> Statement of Account for " + accountVM.SenderAccount.AccountNumber + "</h1>";

            htmlBody += "<table style=\"width:100%;border-collapse:collapse\">";
            foreach (var item in accountVM.TransactionsList)
            {
                htmlBody += "<tr style=\"width:100%;\">";
                htmlBody += "<td style=\"width:20%;border-bottom: 1px solid grey;padding-bottom:10px; padding-top:10px;\">" + item.TransactionDate.ToShortDateString() + "</td>";
                htmlBody += "<td style=\"width:40%;border-bottom: 1px solid grey;padding-bottom:10px; padding-top:10px;\">" + (item.TransactionType == TransactionType.Credit ? item.From : item.To) + "</td>";
                htmlBody += "<td style=\"width:20%;border-bottom: 1px solid grey;padding-bottom:10px; padding-top:10px;\">" + item.Amount + "</td>";
                htmlBody += "<td style=\"width:20%;border-bottom: 1px solid grey;padding-bottom:10px; padding-top:10px;\">" + item.TransactionType + "</td>";
                htmlBody += "</tr>";
            }
            htmlBody += "</table>";



            PdfGenerator.AddPdfPages(document, htmlBody, PageSize.A4);
            byte[]? response = null;

            using (MemoryStream ms = new MemoryStream())
            {
                    document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = $"{accountVM.SenderAccount.AccountNumber}_Statement.pdf";
            return File(response, "application/pdf", Filename);
        }
    }
}
