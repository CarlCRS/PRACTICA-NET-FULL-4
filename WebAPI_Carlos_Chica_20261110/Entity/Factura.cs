namespace WebAPI_Carlos_Chica_20261110.Entity
{
    public class Factura
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string? NumeroFactura { get; set; }
        public DateTime fechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal monto { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string estadopago { get; set; } = "PENDIENTE";
    }
}
