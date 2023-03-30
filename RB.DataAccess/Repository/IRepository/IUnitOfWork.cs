using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        public IBankAccountRepository BankAccountRepo { get; }
        public ITransactionsRepository TransactionsRepo { get; }
        public IApplicationUserRepository ApplicationUserRepo { get; }




        void Save();
    }
}
