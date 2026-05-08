namespace ComandaAi.Web.Application.Models;

public sealed record CarrinhoItemEntrada(
	Guid ProdutoId,
	string NomeProduto,
	decimal PrecoUnitario,
	string? Observacao = null,
	IReadOnlyList<CarrinhoAdicionalState>? Adicionais = null);

public sealed record CarrinhoAdicionalState(Guid AdicionalId, string Nome, decimal PrecoUnitario, int Quantidade)
{
	public decimal Total => PrecoUnitario * Quantidade;
}

public sealed record CarrinhoItemState(
	Guid ItemId,
	Guid ProdutoId,
	string NomeProduto,
	decimal PrecoUnitario,
	int Quantidade,
	string? Observacao,
	IReadOnlyList<CarrinhoAdicionalState> Adicionais)
{
	public decimal Total => (PrecoUnitario * Quantidade) + Adicionais.Sum(adicional => adicional.Total);

	public bool TemPersonalizacao => !string.IsNullOrWhiteSpace(Observacao) || Adicionais.Count > 0;
}