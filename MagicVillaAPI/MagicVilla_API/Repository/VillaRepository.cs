using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository;
public class VillaRepository : Repository<Villa>, IVillaRepository
{
    public readonly ApplicationDbContext _dbContext;

    public VillaRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Villa> Update(Villa entity)
    {
        entity.CreatedDate = DateTime.Now;
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;

    }
}
