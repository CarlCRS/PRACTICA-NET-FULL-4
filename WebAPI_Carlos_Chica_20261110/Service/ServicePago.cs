using WebAPI_Carlos_Chica_20261110.Entity;
using WebAPI_Carlos_Chica_20261110.Repository;

namespace WebAPI_Carlos_Chica_20261110.Service
{
    public class ServicePago
    {
        private readonly PagoRepository _repo;
        private readonly FacturaRepository _repoFactura;

        public ServicePago(PagoRepository repo, FacturaRepository repoFactura)
        {
            _repo = repo;
            _repoFactura = repoFactura;
        }

        public async Task<IEnumerable<Pago>> ObtenerAsync(int? idFactura, int? idCliente)
        {
            return await _repo.ObtenerAsync(idFactura, idCliente);
        }

        public async Task<Pago?> ObtenerPorIdAsync(int id)
        {
            return await _repo.ObtenerPorIdAsync(id);
        }

        public async Task<int> RegistrarAsync(Pago pago, string? rol)
        {
            if (!string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Solo el perfil Administrador puede registrar pagos.");

            if (pago.monto_abonado <= 0)
                throw new InvalidOperationException("El monto abonado debe ser mayor a cero.");

            var factura = await _repoFactura.ObtenerPorIdAsync(pago.id_factura)
                ?? throw new KeyNotFoundException($"No existe la factura {pago.id_factura}.");

            if (factura.SaldoPendiente <= 0)
                throw new InvalidOperationException("La factura ya está cancelada.");

            if (pago.monto_abonado > factura.SaldoPendiente)
                throw new InvalidOperationException(
                    $"El pago supera el saldo pendiente ({factura.SaldoPendiente:N2}).");

            pago.Id = 0;
            if (pago.fecha_pago == default)
                pago.fecha_pago = DateTime.Today;

            return await _repo.RegistrarAsync(pago);
        }
    }
}
