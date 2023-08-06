using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> DbSet;

        public Repository(ApplicationDbContext db)
        {
            DbSet = db.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            var query = DbSet;
            return query.ToList();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            var query = DbSet.Where(filter);
            return query.FirstOrDefault();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
    }
}
