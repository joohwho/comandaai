namespace ComandaAi.Web.Domain.Cardapio;

public sealed record ProdutoCardapio(
	Guid Id,
	string Nome,
	string Descricao,
	decimal PrecoBase,
	ProdutoCardapioTipo Tipo,
	bool Ativo,
	IReadOnlyList<AdicionalCardapio> Adicionais)
{
	public ProdutoCardapio(Guid id, string nome, string descricao, decimal precoBase, ProdutoCardapioTipo tipo, bool ativo, IReadOnlyList<AdicionalCardapio>? adicionais = null) : this()
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("O identificador do produto deve ser valido.", nameof(id));
		}

		if (string.IsNullOrWhiteSpace(nome))
		{
			throw new ArgumentException("O nome do produto deve ser informado.", nameof(nome));
		}

		if (precoBase < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(precoBase), "O preco base do produto nao pode ser negativo.");
		}

		Id = id;
		Nome = nome.Trim();
		Descricao = descricao?.Trim() ?? string.Empty;
		PrecoBase = precoBase;
		Tipo = tipo;
		Ativo = ativo;
		Adicionais = adicionais ?? [];
	}
}