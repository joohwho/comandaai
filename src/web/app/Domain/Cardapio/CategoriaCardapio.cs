namespace ComandaAi.Web.Domain.Cardapio;

public sealed record CategoriaCardapio
{
	public CategoriaCardapio(Guid id, string nome, int ordemExibicao, IReadOnlyList<ProdutoCardapio>? produtos = null)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("O identificador da categoria deve ser valido.", nameof(id));
		}

		if (string.IsNullOrWhiteSpace(nome))
		{
			throw new ArgumentException("O nome da categoria deve ser informado.", nameof(nome));
		}

		if (ordemExibicao < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(ordemExibicao), "A ordem de exibicao nao pode ser negativa.");
		}

		Id = id;
		Nome = nome.Trim();
		OrdemExibicao = ordemExibicao;
		Produtos = produtos ?? [];
	}

	public Guid Id { get; }

	public string Nome { get; }

	public int OrdemExibicao { get; }

	public IReadOnlyList<ProdutoCardapio> Produtos { get; }
}