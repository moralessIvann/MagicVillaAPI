using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository;
public class VillaNumRepository : Repository<VillaNum>, IVillaNumRepository
{
    public readonly ApplicationDbContext _dbContext;

    public VillaNumRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<VillaNum> Update(VillaNum entity)
    {
        entity.CreatedDate = DateTime.Now;
        _dbContext.VillaNum.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;

    }
}
