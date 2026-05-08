using ComandaAi.Web.Domain.Cardapio;
using Xunit;

namespace ComandaAi.Web.Tests.Domain.Cardapio;

public sealed class CategoriaCardapioTests
{
	[Fact]
	public void Constructor_DeveNormalizarNomeEManterProdutosInformados()
	{
		var produto = new ProdutoCardapio(Guid.NewGuid(), "Smash", "Burger da casa", 27.90m, ProdutoCardapioTipo.Lanche, true);

		var categoria = new CategoriaCardapio(Guid.NewGuid(), "  Lanches  ", 3, [produto]);

		Assert.Equal("Lanches", categoria.Nome);
		Assert.Single(categoria.Produtos);
	}

	[Fact]
	public void Constructor_DeveFalharQuandoOrdemForNegativa()
	{
		var action = () => new CategoriaCardapio(Guid.NewGuid(), "Lanches", -1);

		Assert.Throws<ArgumentOutOfRangeException>(action);
	}
}