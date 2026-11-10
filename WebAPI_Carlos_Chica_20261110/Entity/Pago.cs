namespace WebAPI_Carlos_Chica_20261110.Entity
{
    public class Pago
    {
        public int Id { get; set; }
        public int id_factura { get; set; }
        public DateTime fecha_pago { get; set; }
        public decimal monto_abonado { get; set; }
        public string? metodo_pago { get; set; }
        public string? referencia { get; set; }
    }
}
