using Microsoft.AspNetCore.Mvc;
using WebAPI_Carlos_Chica_20261110.Entity;
using WebAPI_Carlos_Chica_20261110.Service;

namespace WebAPI_Carlos_Chica_20261110.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly ServicePago _servicio;

        public PagoController(ServicePago servicio) => _servicio = servicio;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int? idFactura,
            [FromQuery] int? idCliente)
        {
            return Ok(await _servicio.ObtenerAsync(idFactura, idCliente));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var pago = await _servicio.ObtenerPorIdAsync(id);
            if (pago is null)
                return NotFound(new { mensaje = $"No existe el pago {id}." });

            return Ok(pago);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] Pago pago,
            [FromHeader(Name = "X-Rol")] string? rol)
        {
            var id = await _servicio.RegistrarAsync(pago, rol);
            return Created($"/api/pago/{id}", new { id });
        }
    }
}
