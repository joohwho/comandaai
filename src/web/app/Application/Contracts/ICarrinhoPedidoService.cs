using ComandaAi.Web.Application.Models;

namespace ComandaAi.Web.Application.Contracts;

public interface ICarrinhoPedidoService
{
	event Action? Changed;

	IReadOnlyList<CarrinhoItemState> Itens { get; }

	int QuantidadeTotalItens { get; }

	void AdicionarItem(CarrinhoItemEntrada entrada);

	void AdicionarProduto(Guid produtoId, string nomeProduto, decimal precoUnitario);

	void IncrementarQuantidade(Guid itemId);

	void DecrementarQuantidade(Guid itemId);

	void RemoverItem(Guid itemId);

	void SubstituirItens(IReadOnlyList<CarrinhoItemState> itens);

	void Limpar();
}