using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetAllAsync(Guid companyId, EmployeeParameters employeeParameters,bool trackChanges); 
    Task<Employee> GetByIdAsync(Guid companyId, Guid id, bool trackChanges); 
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}