using ComandaAi.Web.Application.Contracts;
using ComandaAi.Web.Application.Models;
using ComandaAi.Web.Domain.Pedidos;

namespace ComandaAi.Web.Application.Services;

public sealed class EnvioPedidoLocalService : IEnvioPedidoService
{
	public Task<EnvioPedidoResultado> EnviarAsync(Pedido pedido, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(pedido);

		if (pedido.Itens.Count == 0)
		{
			throw new InvalidOperationException("Nao e possivel enviar um pedido sem itens.");
		}

		var recebidoEmUtc = DateTimeOffset.UtcNow;
		var protocoloOperacional = $"LOCAL-{recebidoEmUtc:yyyyMMddHHmmss}";

		var resultado = new EnvioPedidoResultado(
			pedido.NumeroExibicao,
			protocoloOperacional,
			PedidoStatus.Recebido,
			recebidoEmUtc,
			"Pedido encaminhado para a fila operacional local.");

		return Task.FromResult(resultado);
	}
}