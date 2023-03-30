using RB.DataAccess.Repository.IRepository;
using RB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ReenBankContext _db;
        public ApplicationUserRepository(ReenBankContext db) : base(db)
        {
            _db = db;
        }
    }
}
