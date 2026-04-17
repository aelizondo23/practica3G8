using System.Text;
using System.Text.Json;
using practica3.Models;

namespace practica3.Services
{
    public class CompraApiService
    {
        private readonly HttpClient _httpClient;

        public CompraApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CompraViewModel>> ConsultarCompras()
        {
            var response = await _httpClient.GetAsync("api/compras");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CompraViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<CompraViewModel>();
        }

        public async Task<List<CompraPendienteViewModel>> ConsultarComprasPendientes()
        {
            var response = await _httpClient.GetAsync("api/compras/pendientes");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CompraPendienteViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<CompraPendienteViewModel>();
        }

        public async Task<SaldoResponseViewModel?> ConsultarSaldo(long idCompra)
        {
            var response = await _httpClient.GetAsync($"api/compras/saldo/{idCompra}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SaldoResponseViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<(bool ok, string mensaje)> RegistrarAbono(AbonoRequestViewModel model)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("api/compras/abonar", content);
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return (true, "Abono registrado correctamente");

            return (false, json);
        }
    }
}