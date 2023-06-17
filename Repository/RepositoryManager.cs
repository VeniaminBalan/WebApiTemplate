using Contracts;
using Entities;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly AppDbContext _appDbContext;

    private ICompanyRepository _companyRepository; 
    private IEmployeeRepository _employeeRepository; 
    
    public RepositoryManager(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public ICompanyRepository Company 
    { 
        get 
        { 
            if(_companyRepository == null) 
                _companyRepository = new CompanyRepository(_appDbContext); 
 
            return _companyRepository; 
        } 
    } 
 
    public IEmployeeRepository Employee 
    { 
        get 
        { 
            if(_employeeRepository == null) 
                _employeeRepository = new EmployeeRepository(_appDbContext); 
 
            return _employeeRepository;  
        } 
    } 
 
    public Task SaveAsync() => _appDbContext.SaveChangesAsync(); 
}