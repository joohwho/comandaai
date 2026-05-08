namespace ComandaAi.Web.Domain.Pedidos;

public sealed record ItemPedidoAdicional
{
	public ItemPedidoAdicional(Guid adicionalId, string nome, decimal precoUnitario, int quantidade)
	{
		if (adicionalId == Guid.Empty)
		{
			throw new ArgumentException("O identificador do adicional deve ser valido.", nameof(adicionalId));
		}

		if (string.IsNullOrWhiteSpace(nome))
		{
			throw new ArgumentException("O nome do adicional deve ser informado.", nameof(nome));
		}

		if (precoUnitario < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(precoUnitario), "O preco do adicional nao pode ser negativo.");
		}

		if (quantidade <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade do adicional deve ser maior que zero.");
		}

		AdicionalId = adicionalId;
		Nome = nome.Trim();
		PrecoUnitario = precoUnitario;
		Quantidade = quantidade;
	}

	public Guid AdicionalId { get; }

	public string Nome { get; }

	public decimal PrecoUnitario { get; }

	public int Quantidade { get; }

	public decimal Total => PrecoUnitario * Quantidade;
}