using DomainLayer.Models;
using System.Linq.Expressions;

namespace RepositoryLayer.IRepository
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        void Insert(T entity);
        void Update(T entity);
       
        void SaveChanges();
        /*when needed*/
        //void Delete(T entity);
        /*when needed*/
        //void Remove(T entity);
    }
}
