using ComandaAi.Web.Application.Services;
using ComandaAi.Web.Domain.Pedidos;
using Xunit;

namespace ComandaAi.Web.Tests.Application.Services;

public sealed class EnvioPedidoLocalServiceTests
{
	[Fact]
	public async Task EnviarAsync_DeveRetornarProtocoloEStatusRecebido()
	{
		var service = new EnvioPedidoLocalService();
		var pedido = CriarPedido();

		var resultado = await service.EnviarAsync(pedido);

		Assert.Equal("A-321", resultado.NumeroPedido);
		Assert.Equal(PedidoStatus.Recebido, resultado.Status);
		Assert.StartsWith("LOCAL-", resultado.ProtocoloOperacional, StringComparison.Ordinal);
	}

	[Fact]
	public async Task EnviarAsync_DeveFalharQuandoPedidoNaoTiverItens()
	{
		var service = new EnvioPedidoLocalService();
		var pedido = new Pedido(
			"A-322",
			new IdentificacaoCliente("Mila"),
			new ReferenciaAtendimento("05", null, "mesa-05"));

		var action = () => service.EnviarAsync(pedido);

		var exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
		Assert.Contains("sem itens", exception.Message, StringComparison.OrdinalIgnoreCase);
	}

	private static Pedido CriarPedido()
	{
		var pedido = new Pedido(
			"A-321",
			new IdentificacaoCliente("Bruno"),
			new ReferenciaAtendimento("03", "C-21", "mesa-03"));

		pedido.AdicionarItem(new ItemPedido(Guid.NewGuid(), "Negroni da Casa", 34.90m, 1));
		return pedido;
	}
}