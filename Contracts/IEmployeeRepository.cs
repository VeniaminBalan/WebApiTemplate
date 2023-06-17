using Entities.Models;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync(Guid companyId, bool trackChanges); 
    Task<Employee> GetByIdAsync(Guid companyId, Guid id, bool trackChanges); 
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}