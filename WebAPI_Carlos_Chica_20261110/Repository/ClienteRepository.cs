using Dapper;
using System.Data;
using WebAPI_Carlos_Chica_20261110.Entity;

namespace WebAPI_Carlos_Chica_20261110.Repository
{
    public class ClienteRepository
    {
        private readonly IDbConnection _conn;

        public ClienteRepository(IDbConnection connection) => _conn = connection;

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            const string sql = @"SELECT Id, nombre, apellido, Cedula, Rol, correo
                                 FROM dbo.Cliente
                                 ORDER BY nombre;";
            return await _conn.QueryAsync<Cliente>(sql);
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            const string sql = @"SELECT Id, nombre, apellido, Cedula, Rol, correo
                                 FROM dbo.Cliente
                                 WHERE Id = @id;";
            return await _conn.QueryFirstOrDefaultAsync<Cliente>(sql, new { id });
        }

        public async Task<bool> ExisteAsync(int id)
        {
            const string sql = "SELECT COUNT(1) FROM dbo.Cliente WHERE Id = @id;";
            return await _conn.ExecuteScalarAsync<int>(sql, new { id }) > 0;
        }
    }
}
