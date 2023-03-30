using RB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.DataAccess.Repository.IRepository
{
    public interface ITransactionsRepository : IRepository<Transactions>
    {
        void Update(Transactions transactions);
    }
}
