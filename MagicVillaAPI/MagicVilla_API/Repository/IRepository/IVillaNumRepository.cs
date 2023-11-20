using MagicVilla_API.Models;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repository.IRepository;

public interface IVillaNumRepository : IRepository<VillaNum>
{
    Task<VillaNum> Update(VillaNum villa);
}
