using ComandaAi.Web.Application.Models;
using ComandaAi.Web.Domain.Pedidos;

namespace ComandaAi.Web.Application.Contracts;

public interface IEnvioPedidoService
{
	Task<EnvioPedidoResultado> EnviarAsync(Pedido pedido, CancellationToken cancellationToken = default);
}