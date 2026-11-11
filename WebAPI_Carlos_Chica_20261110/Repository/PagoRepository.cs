using Dapper;
using System.Data;
using System.Text;
using WebAPI_Carlos_Chica_20261110.Entity;

namespace WebAPI_Carlos_Chica_20261110.Repository
{
    public class PagoRepository
    {
        private readonly IDbConnection _conn;

        public PagoRepository(IDbConnection connection) => _conn = connection;

        public async Task<IEnumerable<Pago>> ObtenerAsync(int? idFactura, int? idCliente)
        {
            var sql = new StringBuilder(@"
                SELECT  p.id_pago AS Id,
                        p.id_factura,
                        p.fecha_pago,
                        p.monto_abonado,
                        p.metodo_pago,
                        p.referencia
                FROM dbo.Pago p
                INNER JOIN dbo.Factura f ON f.Id = p.id_factura
                WHERE 1 = 1");

            if (idFactura.HasValue)
                sql.Append(" AND p.id_factura = @idFactura");
            if (idCliente.HasValue)
                sql.Append(" AND f.IdCliente = @idCliente");

            sql.Append(" ORDER BY p.fecha_pago DESC, p.id_pago DESC;");

            return await _conn.QueryAsync<Pago>(sql.ToString(), new { idFactura, idCliente });
        }

        public async Task<Pago?> ObtenerPorIdAsync(int id)
        {
            const string sql = @"SELECT id_pago AS Id, id_factura, fecha_pago, monto_abonado, metodo_pago, referencia
                                 FROM dbo.Pago
                                 WHERE id_pago = @id;";
            return await _conn.QueryFirstOrDefaultAsync<Pago>(sql, new { id });
        }

        public async Task<int> RegistrarAsync(Pago pago)
        {
            const string sqlActualizarFactura = @"
                UPDATE dbo.Factura
                SET SaldoPendiente = SaldoPendiente - @monto_abonado,
                    estadopago = CASE WHEN SaldoPendiente - @monto_abonado <= 0
                                      THEN 'PAGADA' ELSE 'PARCIAL' END
                WHERE Id = @id_factura
                  AND estadopago <> 'ANULADA'
                  AND SaldoPendiente >= @monto_abonado;";

            const string sqlInsertarPago = @"
                INSERT INTO dbo.Pago (id_factura, fecha_pago, monto_abonado, metodo_pago, referencia)
                VALUES (@id_factura, @fecha_pago, @monto_abonado, @metodo_pago, @referencia);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            if (_conn.State != ConnectionState.Open)
                _conn.Open();

            using var tx = _conn.BeginTransaction();
            try
            {
                var filas = await _conn.ExecuteAsync(sqlActualizarFactura, pago, tx);
                if (filas == 0)
                    throw new InvalidOperationException("El pago no se pudo aplicar: la factura no existe, esta anulada o el monto supera el saldo.");

                var idPago = await _conn.ExecuteScalarAsync<int>(sqlInsertarPago, pago, tx);
                tx.Commit();
                return idPago;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
