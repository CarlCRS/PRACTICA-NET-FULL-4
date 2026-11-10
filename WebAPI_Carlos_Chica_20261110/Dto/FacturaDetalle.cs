namespace WebAPI_Carlos_Chica_20261110.Dto
{
    public class FacturaDetalle
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
    }
}
