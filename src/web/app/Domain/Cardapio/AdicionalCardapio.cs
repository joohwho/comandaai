namespace ComandaAi.Web.Domain.Cardapio;

public sealed record AdicionalCardapio(Guid Id, string Nome, decimal Preco, int QuantidadeMaximaPorItem, bool Obrigatorio)
{
	public AdicionalCardapio(Guid id, string nome, decimal preco, int quantidadeMaximaPorItem, bool obrigatorio = false) : this()
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("O identificador do adicional deve ser valido.", nameof(id));
		}

		if (string.IsNullOrWhiteSpace(nome))
		{
			throw new ArgumentException("O nome do adicional deve ser informado.", nameof(nome));
		}

		if (preco < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(preco), "O preco do adicional nao pode ser negativo.");
		}

		if (quantidadeMaximaPorItem < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(quantidadeMaximaPorItem), "A quantidade maxima do adicional nao pode ser negativa.");
		}

		Id = id;
		Nome = nome.Trim();
		Preco = preco;
		QuantidadeMaximaPorItem = quantidadeMaximaPorItem;
		Obrigatorio = obrigatorio;
	}
}