using ComandaAi.Web.Application.Contracts;
using ComandaAi.Web.Domain.Cardapio;

namespace ComandaAi.Web.Application.Services;

public sealed class CatalogoConsultaService : ICatalogoConsultaService
{
	private static readonly IReadOnlyList<CategoriaCardapio> Categorias =
	[
		new CategoriaCardapio(
			Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E01"),
			"Drinks autorais",
			1,
			[
				new ProdutoCardapio(
					Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E11"),
					"Negroni da Casa",
					"Gin, vermute rosso, bitter e casca de laranja.",
					34.90m,
					ProdutoCardapioTipo.Drink,
					true,
					[
						new AdicionalCardapio(Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E21"), "Dose extra de gin", 8.00m, 1, false),
						new AdicionalCardapio(Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E22"), "Laranja desidratada", 3.50m, 1, false)
					]),
				new ProdutoCardapio(
					Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E12"),
					"Moscow Mule",
					"Vodka, ginger beer, lima e espuma de gengibre.",
					29.90m,
					ProdutoCardapioTipo.Drink,
					true,
					[])
			]),
		new CategoriaCardapio(
			Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E02"),
			"Porcoes",
			2,
			[
				new ProdutoCardapio(
					Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E13"),
					"Batata rustica",
					"Batata crocante com molho da casa.",
					24.90m,
					ProdutoCardapioTipo.Porcao,
					true,
					[
						new AdicionalCardapio(Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E23"), "Cheddar e bacon", 9.50m, 1, false)
					]),
				new ProdutoCardapio(
					Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E14"),
					"Isca de frango",
					"Frango empanado com maionese de ervas.",
					31.90m,
					ProdutoCardapioTipo.Porcao,
					true,
					[])
			]),
		new CategoriaCardapio(
			Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E03"),
			"Lanches",
			3,
			[
				new ProdutoCardapio(
					Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E15"),
					"Smash da casa",
					"Pao brioche, burger de 120g, queijo e maionese especial.",
					27.90m,
					ProdutoCardapioTipo.Lanche,
					true,
					[
						new AdicionalCardapio(Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E24"), "Burger extra", 10.00m, 2, false),
						new AdicionalCardapio(Guid.Parse("7D8D8F5D-6080-4E4C-A0FB-6F6AFA8E0E25"), "Queijo extra", 4.50m, 2, false)
					])
			])
	];

	public Task<IReadOnlyList<CategoriaCardapio>> ListarCategoriasAsync(CancellationToken cancellationToken = default)
	{
		return Task.FromResult(Categorias);
	}
}