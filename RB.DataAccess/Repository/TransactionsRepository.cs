using RB.DataAccess.Repository.IRepository;
using RB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RB.DataAccess.Repository
{
    public class TransactionsRepository : Repository<Transactions>, ITransactionsRepository
    {
        private readonly ReenBankContext _db;
        public TransactionsRepository(ReenBankContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Transactions transactions)
        {
            _db.Transactions.Update(transactions);
        }
    }
}
