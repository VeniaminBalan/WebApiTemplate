using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
    public async Task<IEnumerable<Company>> GetAllAync(bool trackChanges)=>
        await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();
    public async Task<Company> GetByIdAsync(Guid companyId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(companyId), trackChanges) 
            .SingleOrDefaultAsync();
    public async Task<IEnumerable<Company>> getByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

    
    public void CreateCompany(Company company) => Create(company);
    
    public void DeleteCompany(Company company)
    {
        Delete(company);
    }
    

    

}