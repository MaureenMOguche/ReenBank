using RB.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReenBankContext _db;
        public UnitOfWork(ReenBankContext db)
        {
            _db = db;
            BankAccountRepo = new BankAccountRepository(db);
            TransactionsRepo = new TransactionsRepository(db);
            ApplicationUserRepo = new ApplicationUserRepository(db);
        }

        public IBankAccountRepository BankAccountRepo { get; private set; }

        public ITransactionsRepository TransactionsRepo { get; private set; }
        public IApplicationUserRepository ApplicationUserRepo { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
