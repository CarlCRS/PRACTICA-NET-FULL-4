namespace WebAPI_Carlos_Chica_20261110.Entity
{
    public class Cliente
    {
        public int Id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string? correo { get; set; }
    }
}
