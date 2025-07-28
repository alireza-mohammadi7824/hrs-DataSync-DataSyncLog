
using HRSDataIntegration.EntityFrameworkCore.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HRSDataIntegration.EntityFrameworkCore
{
    public class RepositoryBaseOracle<T>: IOracleRepository<T> where T : class, new()
    {
         private readonly OracleDbContext _context;

        public RepositoryBaseOracle(OracleDbContext context)
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

        public T Get(string id)
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

        public void Delete(T entity)
        {
           _context.Set<T>().Remove(entity);
        }

        public void CreateList(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
                throw new ArgumentException("لیست ورودی نمی‌تواند خالی باشد!");

            _context.Set<T>().AddRange(entities);
        }

        public void DeleteList(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
                throw new ArgumentException("لیست ورودی نمی‌تواند خالی باشد!");

            _context.Set<T>().RemoveRange(entities);
        }

    }
}
