using ComandaAi.Web.Domain.Pedidos;
using Xunit;

namespace ComandaAi.Web.Tests.Domain.Pedidos;

public sealed class PedidoTests
{
	[Fact]
	public void AdicionarItem_DeveSomarTotalDoPedidoComAdicionais()
	{
		var pedido = CriarPedido();
		var item = new ItemPedido(
			Guid.NewGuid(),
			"Negroni",
			34.90m,
			2,
			null,
			[
				new ItemPedidoAdicional(Guid.NewGuid(), "Dose extra", 8.00m, 1)
			]);

		pedido.AdicionarItem(item);

		Assert.Single(pedido.Itens);
		Assert.Equal(77.80m, pedido.Total);
	}

	[Fact]
	public void AtualizarStatus_DeveRejeitarTransicaoInvalida()
	{
		var pedido = CriarPedido();

		var action = () => pedido.AtualizarStatus(PedidoStatus.Entregue);

		var exception = Assert.Throws<InvalidOperationException>(action);
		Assert.Contains("nao e permitida", exception.Message, StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public void AdicionarItem_DeveFalharQuandoPedidoEstiverCancelado()
	{
		var pedido = CriarPedido();
		pedido.AtualizarStatus(PedidoStatus.Cancelado);

		var action = () => pedido.AdicionarItem(new ItemPedido(Guid.NewGuid(), "Batata", 24.90m, 1));

		var exception = Assert.Throws<InvalidOperationException>(action);
		Assert.Contains("pedido encerrado", exception.Message, StringComparison.OrdinalIgnoreCase);
	}

	private static Pedido CriarPedido()
	{
		return new Pedido(
			"A-100",
			new IdentificacaoCliente("Ana"),
			new ReferenciaAtendimento("08", "C-14", "mesa-08"));
	}
}