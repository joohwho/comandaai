namespace ComandaAi.Web.Domain.Pedidos;

public sealed class ItemPedido
{
	private readonly List<ItemPedidoAdicional> adicionais;

	public ItemPedido(Guid produtoId, string nomeProduto, decimal precoUnitario, int quantidade, string? observacao = null, IEnumerable<ItemPedidoAdicional>? adicionais = null)
	{
		if (produtoId == Guid.Empty)
		{
			throw new ArgumentException("O identificador do produto deve ser valido.", nameof(produtoId));
		}

		if (string.IsNullOrWhiteSpace(nomeProduto))
		{
			throw new ArgumentException("O nome do produto deve ser informado.", nameof(nomeProduto));
		}

		if (precoUnitario < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(precoUnitario), "O preco unitario nao pode ser negativo.");
		}

		if (quantidade <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade do item deve ser maior que zero.");
		}

		ProdutoId = produtoId;
		NomeProduto = nomeProduto.Trim();
		PrecoUnitario = precoUnitario;
		Quantidade = quantidade;
		Observacao = string.IsNullOrWhiteSpace(observacao) ? null : observacao.Trim();
		this.adicionais = adicionais?.ToList() ?? [];
	}

	public Guid ProdutoId { get; }

	public string NomeProduto { get; }

	public decimal PrecoUnitario { get; }

	public int Quantidade { get; }

	public string? Observacao { get; }

	public IReadOnlyList<ItemPedidoAdicional> Adicionais => adicionais;

	public decimal Total => (PrecoUnitario * Quantidade) + adicionais.Sum(adicional => adicional.Total);
}