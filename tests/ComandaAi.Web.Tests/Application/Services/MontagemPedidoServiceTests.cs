using ComandaAi.Web.Application.Contracts;
using ComandaAi.Web.Application.Models;
using ComandaAi.Web.Application.Services;
using ComandaAi.Web.Domain.Cardapio;
using Xunit;

namespace ComandaAi.Web.Tests.Application.Services;

public sealed class MontagemPedidoServiceTests
{
	[Fact]
	public async Task MontarAsync_DeveMontarPedidoComTotalCorreto()
	{
		var service = new MontagemPedidoService(new CatalogoConsultaFakeService(CriarCatalogo()));
		var entrada = new MontagemPedidoEntrada(
			"A-201",
			"Carla",
			"10",
			null,
			"mesa-10",
			[
				new MontagemPedidoItemEntrada(
					ProdutoValidoId,
					2,
					"Sem cebola",
					[
						new MontagemPedidoAdicionalEntrada(AdicionalValidoId, 1)
					])
			]);

		var pedido = await service.MontarAsync(entrada);

		Assert.Equal("A-201", pedido.NumeroExibicao);
		Assert.Equal("Carla", pedido.Cliente.NomeExibicao);
		Assert.Single(pedido.Itens);
		Assert.Equal(52.00m, pedido.Total);
	}

	[Fact]
	public async Task MontarAsync_DeveFalharQuandoProdutoNaoExistir()
	{
		var service = new MontagemPedidoService(new CatalogoConsultaFakeService(CriarCatalogo()));
		var entrada = new MontagemPedidoEntrada(
			"A-202",
			"Diego",
			"02",
			null,
			"mesa-02",
			[
				new MontagemPedidoItemEntrada(Guid.NewGuid(), 1)
			]);

		var action = () => service.MontarAsync(entrada);

		var exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
		Assert.Contains("produto informado", exception.Message, StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public async Task MontarAsync_DeveFalharQuandoAdicionalExcederLimite()
	{
		var service = new MontagemPedidoService(new CatalogoConsultaFakeService(CriarCatalogo()));
		var entrada = new MontagemPedidoEntrada(
			"A-203",
			"Elisa",
			null,
			"C-8",
			"comanda-c-8",
			[
				new MontagemPedidoItemEntrada(
					ProdutoValidoId,
					1,
					null,
					[
						new MontagemPedidoAdicionalEntrada(AdicionalValidoId, 3)
					])
			]);

		var action = () => service.MontarAsync(entrada);

		var exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
		Assert.Contains("excede o limite", exception.Message, StringComparison.OrdinalIgnoreCase);
	}

	private static readonly Guid ProdutoValidoId = Guid.Parse("B58D77A7-3AF7-4A04-9834-C8B903B1D111");
	private static readonly Guid AdicionalValidoId = Guid.Parse("B58D77A7-3AF7-4A04-9834-C8B903B1D222");

	private static IReadOnlyList<CategoriaCardapio> CriarCatalogo()
	{
		return
		[
			new CategoriaCardapio(
				Guid.Parse("B58D77A7-3AF7-4A04-9834-C8B903B1D001"),
				"Lanches",
				1,
				[
					new ProdutoCardapio(
						ProdutoValidoId,
						"Burger da casa",
						"Pao, burger e queijo.",
						22.00m,
						ProdutoCardapioTipo.Lanche,
						true,
						[
							new AdicionalCardapio(AdicionalValidoId, "Queijo extra", 8.00m, 2, false)
						])
				])
		];
	}

	private sealed class CatalogoConsultaFakeService(IReadOnlyList<CategoriaCardapio> categorias) : ICatalogoConsultaService
	{
		public Task<IReadOnlyList<CategoriaCardapio>> ListarCategoriasAsync(CancellationToken cancellationToken = default)
		{
			return Task.FromResult(categorias);
		}
	}
}