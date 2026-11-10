namespace WebAPI_Carlos_Chica_20261110.Dto
{
    public class ResumenCartera
    {
        public int IdCliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal TotalFacturado { get; set; }
        public decimal TotalCobrado { get; set; }
        public decimal SaldoPendiente { get; set; }
    }
}
