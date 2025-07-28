using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HRSDataIntegration
{
    public interface IOracleRepository<T> where T : class
    {
        T Get(string id);
        List<T> Get();
        void Create(T? entity);
        void CreateList(List<T> entities); 
        void Delete(T entity);
        void DeleteList(List<T> entities);
        bool Exists(Expression<Func<T, bool>> expression);
        IQueryable<T> GetQueryable();
        void SaveChanges();
    }
}
