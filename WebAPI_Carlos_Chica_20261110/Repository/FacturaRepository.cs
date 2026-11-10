using Dapper;
using System.Data;
using System.Text;
using WebAPI_Carlos_Chica_20261110.Dto;
using WebAPI_Carlos_Chica_20261110.Entity;

namespace WebAPI_Carlos_Chica_20261110.Repository
{
    public class FacturaRepository
    {
        private readonly IDbConnection _conn;

        public FacturaRepository(IDbConnection connection) => _conn = connection;

        private const string SelectFactura = @"
            SELECT  f.Id,
                    f.IdCliente,
                    c.nombre AS NombreCliente,
                    f.NumeroFactura,
                    f.fechaEmision,
                    f.FechaVencimiento,
                    f.monto,
                    f.SaldoPendiente,
                    f.estadopago
            FROM dbo.Factura f
            INNER JOIN dbo.Cliente c ON c.Id = f.IdCliente";

        public async Task<IEnumerable<FacturaDetalle>> ObtenerAsync(int? idCliente)
        {
            var sql = new StringBuilder(SelectFactura);
            sql.Append(" WHERE 1 = 1");

            if (idCliente.HasValue)
                sql.Append(" AND f.IdCliente = @idCliente");

            sql.Append(" ORDER BY f.FechaVencimiento;");

            return await _conn.QueryAsync<FacturaDetalle>(
                sql.ToString(), new { idCliente});
        }

        public async Task<FacturaDetalle?> ObtenerPorIdAsync(int id)
        {
            var sql = SelectFactura + " WHERE f.Id = @id;";
            return await _conn.QueryFirstOrDefaultAsync<FacturaDetalle>(sql, new { id });
        }

        public async Task<bool> ExisteNumeroAsync(string numeroFactura, int? excluirId = null)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.Factura
                                 WHERE NumeroFactura = @numeroFactura
                                   AND (@excluirId IS NULL OR Id <> @excluirId);";
            return await _conn.ExecuteScalarAsync<int>(sql, new { numeroFactura, excluirId }) > 0;
        }

        public async Task<decimal> ObtenerTotalPagadoAsync(int idFactura)
        {
            const string sql = @"SELECT ISNULL(SUM(monto_abonado), 0)
                                 FROM dbo.Pago
                                 WHERE id_factura = @idFactura;";
            return await _conn.ExecuteScalarAsync<decimal>(sql, new { idFactura });
        }

        public async Task<int> InsertarAsync(Factura factura)
        {
            const string sql = @"
                INSERT INTO dbo.Factura
                    (IdCliente, NumeroFactura, fechaEmision, FechaVencimiento, monto, SaldoPendiente, estadopago)
                VALUES
                    (@IdCliente, @NumeroFactura, @fechaEmision, @FechaVencimiento, @monto, @SaldoPendiente, @estadopago);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            return await _conn.ExecuteScalarAsync<int>(sql, factura);
        }

        public async Task<bool> ActualizarAsync(Factura factura)
        {
            const string sql = @"
                UPDATE dbo.Factura
                SET NumeroFactura    = @NumeroFactura,
                    fechaEmision     = @fechaEmision,
                    FechaVencimiento = @FechaVencimiento,
                    monto            = @monto,
                    SaldoPendiente   = @SaldoPendiente,
                    estadopago       = @estadopago
                WHERE Id = @Id;";
            return await _conn.ExecuteAsync(sql, factura) > 0;
        }
    }
}
