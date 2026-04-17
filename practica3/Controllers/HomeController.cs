using Microsoft.AspNetCore.Mvc;
using practica3.Models;
using practica3.Services;


namespace practica3.Controllers
{
    public class HomeController : Controller
    {
        private readonly CompraApiService _apiService;

        public HomeController(CompraApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Consulta()
        {
            var compras = await _apiService.ConsultarCompras();
            return View(compras);
        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            var model = new RegistroAbonoViewModel
            {
                ComprasPendientes = await _apiService.ConsultarComprasPendientes()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroAbonoViewModel model)
        {
            model.ComprasPendientes = await _apiService.ConsultarComprasPendientes();

            if (!ModelState.IsValid)
                return View(model);

            var saldo = await _apiService.ConsultarSaldo(model.Id_Compra);

            if (saldo == null)
            {
                ModelState.AddModelError("", "No se pudo consultar el saldo.");
                return View(model);
            }

            if (model.Monto > saldo.Saldo)
            {
                ModelState.AddModelError("", "El abono no puede ser mayor al saldo anterior.");
                model.SaldoAnterior = saldo.Saldo;
                return View(model);
            }

            var response = await _apiService.RegistrarAbono(new AbonoRequestViewModel
            {
                Id_Compra = model.Id_Compra,
                Monto = model.Monto
            });

            if (!response.ok)
            {
                ModelState.AddModelError("", "No se pudo registrar el abono.");
                model.SaldoAnterior = saldo.Saldo;
                return View(model);
            }

            return RedirectToAction("Consulta");
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerSaldo(long idCompra)
        {
            var saldo = await _apiService.ConsultarSaldo(idCompra);

            if (saldo == null)
                return Json(new { ok = false, mensaje = "Compra no encontrada" });

            return Json(new { ok = true, saldo = saldo.Saldo });
        }
    }
}