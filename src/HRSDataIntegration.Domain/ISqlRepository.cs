using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HRSDataIntegration
{
    public interface ISqlRepository<T> where T : class
    {
        T Get(Guid id);
        List<T> Get();
        void Create(T? entity);
        bool Exists(Expression<Func<T, bool>> expression);
        IQueryable<T> GetQueryable();
        void SaveChanges();
    }
}
