using Microsoft.AspNetCore.Mvc;
using practica3_API.Data;
using practica3_API.Models;


namespace PracticaS13_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly CompraRepository _repository;

        public ComprasController(CompraRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompras()
        {
            var data = await _repository.ConsultarCompras();
            return Ok(data);
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes()
        {
            var data = await _repository.ConsultarComprasPendientes();
            return Ok(data);
        }

        [HttpGet("saldo/{id}")]
        public async Task<IActionResult> GetSaldo(long id)
        {
            var data = await _repository.ConsultarSaldoCompra(id);

            if (data == null)
                return NotFound(new { mensaje = "Compra no encontrada" });

            return Ok(data);
        }

        [HttpPost("abonar")]
        public async Task<IActionResult> Abonar([FromBody] AbonoRequest request)
        {
            try
            {
                await _repository.RegistrarAbono(request.Id_Compra, request.Monto);
                return Ok(new { mensaje = "Abono registrado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}