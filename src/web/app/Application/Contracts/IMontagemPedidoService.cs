using ComandaAi.Web.Application.Models;
using ComandaAi.Web.Domain.Pedidos;

namespace ComandaAi.Web.Application.Contracts;

public interface IMontagemPedidoService
{
	Task<Pedido> MontarAsync(MontagemPedidoEntrada entrada, CancellationToken cancellationToken = default);
}