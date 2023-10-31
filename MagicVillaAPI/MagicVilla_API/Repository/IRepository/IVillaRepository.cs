using MagicVilla_API.Models;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repository.IRepository;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> Update(Villa villa);
}
