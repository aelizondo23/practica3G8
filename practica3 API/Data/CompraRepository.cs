using System.Data;
using Dapper;
using practica3_API.Models;

namespace practica3_API.Data
{
    public class CompraRepository
    {
        private readonly DapperContext _context;

        public CompraRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Compra>> ConsultarCompras()
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<Compra>(
                "ConsultarCompras",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<CompraPendiente>> ConsultarComprasPendientes()
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<CompraPendiente>(
                "ConsultarComprasPendientes",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SaldoResponse?> ConsultarSaldoCompra(long idCompra)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id_Compra", idCompra);

            return await connection.QueryFirstOrDefaultAsync<SaldoResponse>(
                "ConsultarSaldoCompra",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RegistrarAbono(long idCompra, decimal monto)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id_Compra", idCompra);
            parameters.Add("@Monto", monto);

            await connection.ExecuteAsync(
                "RegistrarAbono",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}