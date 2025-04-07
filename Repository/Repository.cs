using System.Linq.Expressions;
using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _appDbContext;
    protected Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IQueryable<T> FindAll(bool trackChanges) => 
        !trackChanges ? 
            _appDbContext.Set<T>() 
                .AsNoTracking() : 
            _appDbContext.Set<T>(); 
    
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => 
        !trackChanges ? 
            _appDbContext.Set<T>() 
                .Where(expression) 
                .AsNoTracking() : 
            _appDbContext.Set<T>() 
                .Where(expression);  

    public void Create(T entity) => _appDbContext.Set<T>().Add(entity); 
    public void Update(T entity) => _appDbContext.Set<T>().Update(entity); 
    public void Delete(T entity) => _appDbContext.Set<T>().Remove(entity); 
}