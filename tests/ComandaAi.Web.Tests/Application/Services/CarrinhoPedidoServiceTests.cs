using ComandaAi.Web.Application.Models;
using ComandaAi.Web.Application.Services;
using Xunit;

namespace ComandaAi.Web.Tests.Application.Services;

public sealed class CarrinhoPedidoServiceTests
{
	[Fact]
	public void AdicionarProduto_DeveSomarQuantidadeDoMesmoProduto()
	{
		var service = new CarrinhoPedidoService();
		var produtoId = Guid.NewGuid();

		service.AdicionarProduto(produtoId, "Burger da casa", 22.00m);
		service.AdicionarProduto(produtoId, "Burger da casa", 22.00m);

		var item = Assert.Single(service.Itens);
		Assert.Equal(2, item.Quantidade);
		Assert.Equal(44.00m, item.Total);
	}

	[Fact]
	public void AdicionarItem_DeveCriarLinhasSeparadasQuandoHouverObservacao()
	{
		var service = new CarrinhoPedidoService();
		var produtoId = Guid.NewGuid();

		service.AdicionarItem(new CarrinhoItemEntrada(produtoId, "Burger da casa", 22.00m, "Sem cebola"));
		service.AdicionarItem(new CarrinhoItemEntrada(produtoId, "Burger da casa", 22.00m, "Ponto mais passado"));

		Assert.Equal(2, service.Itens.Count);
		Assert.All(service.Itens, item => Assert.Equal(1, item.Quantidade));
	}

	[Fact]
	public void AdicionarItem_DeveManterAdicionaisNoTotalDoCarrinho()
	{
		var service = new CarrinhoPedidoService();

		service.AdicionarItem(new CarrinhoItemEntrada(
			Guid.NewGuid(),
			"Batata rustica",
			24.90m,
			null,
			[
				new CarrinhoAdicionalState(Guid.NewGuid(), "Cheddar e bacon", 9.50m, 1)
			]));

		var item = Assert.Single(service.Itens);
		Assert.Equal(34.40m, item.Total);
		Assert.Single(item.Adicionais);
	}

	[Fact]
	public void DecrementarQuantidade_DeveReduzirQuantidadeEAposRemoverUltimoExcluirItem()
	{
		var service = new CarrinhoPedidoService();
		var produtoId = Guid.NewGuid();

		service.AdicionarProduto(produtoId, "Batata rustica", 24.90m);
		service.AdicionarProduto(produtoId, "Batata rustica", 24.90m);
		var itemId = service.Itens[0].ItemId;

		service.DecrementarQuantidade(itemId);
		Assert.Single(service.Itens);
		Assert.Equal(1, service.Itens[0].Quantidade);

		service.DecrementarQuantidade(itemId);
		Assert.Empty(service.Itens);
	}

	[Fact]
	public void Limpar_DeveRemoverTodosOsItens()
	{
		var service = new CarrinhoPedidoService();
		service.AdicionarProduto(Guid.NewGuid(), "Negroni", 34.90m);
		service.AdicionarProduto(Guid.NewGuid(), "Isca de frango", 31.90m);

		service.Limpar();

		Assert.Empty(service.Itens);
		Assert.Equal(0, service.QuantidadeTotalItens);
	}

	[Fact]
	public void SubstituirItens_DeveRestaurarEstadoDoCarrinho()
	{
		var service = new CarrinhoPedidoService();
		var itens = new[]
		{
			new CarrinhoItemState(
				Guid.NewGuid(),
				Guid.NewGuid(),
				"Burger da casa",
				22.00m,
				2,
				"Sem cebola",
				[])
		};

		service.SubstituirItens(itens);

		var item = Assert.Single(service.Itens);
		Assert.Equal(2, service.QuantidadeTotalItens);
		Assert.Equal("Sem cebola", item.Observacao);
	}
}