using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }


    public IEnumerable<Company> GetAll(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();

    public Company GetCompany(Guid companyId, bool trackChanges) => 
        FindByCondition(c => c.Id.Equals(companyId), trackChanges) 
            .SingleOrDefault();

    public void CreateCompany(Company company) => Create(company);

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToList();
}