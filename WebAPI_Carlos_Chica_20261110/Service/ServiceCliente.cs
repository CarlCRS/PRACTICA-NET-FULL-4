using WebAPI_Carlos_Chica_20261110.Dto;
using WebAPI_Carlos_Chica_20261110.Entity;
using WebAPI_Carlos_Chica_20261110.Repository;

namespace WebAPI_Carlos_Chica_20261110.Service
{
    public class ServiceCliente
    {
        private readonly ClienteRepository _repo;

        public ServiceCliente(ClienteRepository repo) => _repo = repo;

        public async Task<IEnumerable<Cliente>> ObtenerAsync()
        {
            return await _repo.ObtenerTodosAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _repo.ObtenerPorIdAsync(id);
        }

 
    }
}
