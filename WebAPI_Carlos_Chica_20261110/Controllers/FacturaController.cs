using Microsoft.AspNetCore.Mvc;
using WebAPI_Carlos_Chica_20261110.Entity;
using WebAPI_Carlos_Chica_20261110.Service;

namespace WebAPI_Carlos_Chica_20261110.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly ServiceFactura _servicio;

        public FacturaController(ServiceFactura servicio) => _servicio = servicio;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? idCliente)
        {
            return Ok(await _servicio.ObtenerAsync(idCliente));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var factura = await _servicio.ObtenerPorIdAsync(id);
            if (factura is null)
                return NotFound(new { mensaje = $"No existe la factura {id}." });

            return Ok(factura);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] Factura factura,
            [FromHeader(Name = "Rol")] string? rol)
        {
            var id = await _servicio.RegistrarAsync(factura, rol);
            return Created($"/api/factura/{id}", new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] Factura factura,
            [FromHeader(Name = "Rol")] string? rol)
        {
            await _servicio.ActualizarAsync(id, factura, rol);
            return NoContent();
        }
    }
}
