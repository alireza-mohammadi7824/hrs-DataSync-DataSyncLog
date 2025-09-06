
using HRSDataIntegration.EntityFrameworkCore.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HRSDataIntegration.EntityFrameworkCore
{
    public class RepositoryBaseSQL<T>: ISqlRepository<T> where T : class
    {
         private readonly HRSDataIntegrationDbContext _context;

        public RepositoryBaseSQL(HRSDataIntegrationDbContext context)
        {
            _context = context;
        }

        public void Create(T? entity)
        {
            _context.Add(entity);

        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }

        public T Get(Guid id)
        {
            return _context.Find<T>(id);
        }
        

        public List<T> Get()
        {
            return _context.Set<T>().ToList();
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
