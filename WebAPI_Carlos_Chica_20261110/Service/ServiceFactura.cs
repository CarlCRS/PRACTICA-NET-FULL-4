using WebAPI_Carlos_Chica_20261110.Dto;
using WebAPI_Carlos_Chica_20261110.Entity;
using WebAPI_Carlos_Chica_20261110.Repository;

namespace WebAPI_Carlos_Chica_20261110.Service
{
    public class ServiceFactura
    {
        private readonly FacturaRepository _repo;
        private readonly ClienteRepository _repoCliente;

        public ServiceFactura(FacturaRepository repo, ClienteRepository repoCliente)
        {
            _repo = repo;
            _repoCliente = repoCliente;
        }

        public async Task<IEnumerable<FacturaDetalle>> ObtenerAsync(int? idCliente)
        {
            return await _repo.ObtenerAsync();
        }

        public async Task<FacturaDetalle?> ObtenerPorIdAsync(int id)
        {
            return await _repo.ObtenerPorIdAsync(id);
        }

        public async Task<int> RegistrarAsync(Factura factura, string? rol)
        {
            ValidarAdministrador(rol);
            Validar(factura);

            if (!await _repoCliente.ExisteAsync(factura.IdCliente))
                throw new KeyNotFoundException($"No existe el cliente {factura.IdCliente}.");

            if (await _repo.ExisteNumeroAsync(factura.NumeroFactura))
                throw new InvalidOperationException($"Ya existe la factura {factura.NumeroFactura}.");

            factura.Id = 0;
            factura.SaldoPendiente = factura.monto;
            factura.estadopago = "PENDIENTE";

            return await _repo.InsertarAsync(factura);
        }

        public async Task ActualizarAsync(int id, Factura factura, string? rol)
        {
            ValidarAdministrador(rol);
            Validar(factura);

            var actual = await _repo.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"No existe la factura {id}.");

            if (actual.IdCliente != factura.IdCliente)
                throw new InvalidOperationException("No se puede cambiar la factura de cliente.");

            if (await _repo.ExisteNumeroAsync(factura.NumeroFactura, id))
                throw new InvalidOperationException($"Ya existe otra factura con el número {factura.NumeroFactura}.");

            var totalPagado = await _repo.ObtenerTotalPagadoAsync(id);
            if (factura.monto < totalPagado)
                throw new InvalidOperationException($"El monto no puede ser menor a lo ya pagado ({totalPagado:N2}).");

            factura.Id = actual.Id;
            factura.SaldoPendiente = factura.monto - totalPagado;
            factura.estadopago = factura.SaldoPendiente <= 0
                ? "PAGADA"
                : totalPagado > 0 ? "PARCIAL" : "PENDIENTE";

            await _repo.ActualizarAsync(factura);
        }

        private static void Validar(Factura factura)
        {
            if (factura.monto <= 0)
                throw new InvalidOperationException("El monto debe ser mayor a cero.");

            if (factura.FechaVencimiento.Date < factura.fechaEmision.Date)
                throw new InvalidOperationException("La fecha de vencimiento no puede ser menor a la de emisión.");
        }

        private static void ValidarAdministrador(string? rol)
        {
            if (!string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Solo el perfil Administrador puede gestionar facturas.");
        }
    }
}
