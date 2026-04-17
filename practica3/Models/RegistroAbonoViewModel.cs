using System.ComponentModel.DataAnnotations;
using practica3.Models;

namespace practica3.Models
{
    public class RegistroAbonoViewModel
    {
        [Required]
        public long Id_Compra { get; set; }

        public decimal SaldoAnterior { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ingrese un abono válido")]
        public decimal Monto { get; set; }

        public List<CompraPendienteViewModel> ComprasPendientes { get; set; } = new();
    }
}