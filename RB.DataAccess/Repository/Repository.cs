using Microsoft.EntityFrameworkCore;
using RB.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RB.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ReenBankContext _db;
        public DbSet<T> dbset { get; set; }
        public Repository(ReenBankContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbset;
            return query.ToList();
        }

        public T GetOne(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }
    }
}
