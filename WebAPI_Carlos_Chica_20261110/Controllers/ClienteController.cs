using Microsoft.AspNetCore.Mvc;
using WebAPI_Carlos_Chica_20261110.Service;

namespace WebAPI_Carlos_Chica_20261110.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ServiceCliente _servicio;

        public ClienteController(ServiceCliente servicio) => _servicio = servicio;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _servicio.ObtenerAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await _servicio.ObtenerPorIdAsync(id);
            if (cliente is null)
                return NotFound(new { mensaje = $"No existe el cliente {id}." });

            return Ok(cliente);
        }
    }
}
