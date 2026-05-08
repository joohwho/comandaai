using ComandaAi.Web.Application.Contracts;
using ComandaAi.Web.Application.Models;

namespace ComandaAi.Web.Application.Services;

public sealed class CarrinhoPedidoService : ICarrinhoPedidoService
{
	private readonly List<CarrinhoItemState> itens = [];

	public event Action? Changed;

	public IReadOnlyList<CarrinhoItemState> Itens => itens;

	public int QuantidadeTotalItens => itens.Sum(item => item.Quantidade);

	public void AdicionarItem(CarrinhoItemEntrada entrada)
	{
		ArgumentNullException.ThrowIfNull(entrada);

		if (entrada.ProdutoId == Guid.Empty)
		{
			throw new ArgumentException("O identificador do produto deve ser valido.", nameof(entrada));
		}

		if (string.IsNullOrWhiteSpace(entrada.NomeProduto))
		{
			throw new ArgumentException("O nome do produto deve ser informado.", nameof(entrada));
		}

		if (entrada.PrecoUnitario < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(entrada), "O preco do produto nao pode ser negativo.");
		}

		var observacaoNormalizada = string.IsNullOrWhiteSpace(entrada.Observacao) ? null : entrada.Observacao.Trim();
		var adicionaisNormalizados = NormalizarAdicionais(entrada.Adicionais);
		var itemExistente = observacaoNormalizada is null && adicionaisNormalizados.Count == 0
			? itens.FirstOrDefault(item => item.ProdutoId == entrada.ProdutoId && !item.TemPersonalizacao)
			: null;

		if (itemExistente is null)
		{
			itens.Add(new CarrinhoItemState(
				Guid.NewGuid(),
				entrada.ProdutoId,
				entrada.NomeProduto.Trim(),
				entrada.PrecoUnitario,
				1,
				observacaoNormalizada,
				adicionaisNormalizados));
		}
		else
		{
			var indice = itens.IndexOf(itemExistente);
			itens[indice] = itemExistente with { Quantidade = itemExistente.Quantidade + 1 };
		}

		Changed?.Invoke();
	}

	public void AdicionarProduto(Guid produtoId, string nomeProduto, decimal precoUnitario)
	{
		AdicionarItem(new CarrinhoItemEntrada(produtoId, nomeProduto, precoUnitario));
	}

	public void IncrementarQuantidade(Guid itemId)
	{
		var itemExistente = itens.FirstOrDefault(item => item.ItemId == itemId);

		if (itemExistente is null)
		{
			return;
		}

		if (itemExistente.TemPersonalizacao)
		{
			itens.Add(itemExistente with { ItemId = Guid.NewGuid(), Quantidade = 1 });
		}
		else
		{
			var indice = itens.IndexOf(itemExistente);
			itens[indice] = itemExistente with { Quantidade = itemExistente.Quantidade + 1 };
		}

		Changed?.Invoke();
	}

	public void DecrementarQuantidade(Guid itemId)
	{
		var itemExistente = itens.FirstOrDefault(item => item.ItemId == itemId);

		if (itemExistente is null)
		{
			return;
		}

		if (itemExistente.TemPersonalizacao || itemExistente.Quantidade == 1)
		{
			itens.Remove(itemExistente);
		}
		else
		{
			var indice = itens.IndexOf(itemExistente);
			itens[indice] = itemExistente with { Quantidade = itemExistente.Quantidade - 1 };
		}

		Changed?.Invoke();
	}

	public void RemoverItem(Guid itemId)
	{
		var itemExistente = itens.FirstOrDefault(item => item.ItemId == itemId);

		if (itemExistente is null)
		{
			return;
		}

		itens.Remove(itemExistente);
		Changed?.Invoke();
	}

	public void SubstituirItens(IReadOnlyList<CarrinhoItemState> itens)
	{
		ArgumentNullException.ThrowIfNull(itens);

		this.itens.Clear();
		this.itens.AddRange(itens.Where(item => item.Quantidade > 0));
		Changed?.Invoke();
	}

	public void Limpar()
	{
		if (itens.Count == 0)
		{
			return;
		}

		itens.Clear();
		Changed?.Invoke();
	}

	private static IReadOnlyList<CarrinhoAdicionalState> NormalizarAdicionais(IReadOnlyList<CarrinhoAdicionalState>? adicionais)
	{
		if (adicionais is null || adicionais.Count == 0)
		{
			return [];
		}

		var normalizados = new List<CarrinhoAdicionalState>(adicionais.Count);

		foreach (var adicional in adicionais)
		{
			if (adicional.AdicionalId == Guid.Empty)
			{
				throw new ArgumentException("O identificador do adicional deve ser valido.", nameof(adicionais));
			}

			if (string.IsNullOrWhiteSpace(adicional.Nome))
			{
				throw new ArgumentException("O nome do adicional deve ser informado.", nameof(adicionais));
			}

			if (adicional.PrecoUnitario < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(adicionais), "O preco do adicional nao pode ser negativo.");
			}

			if (adicional.Quantidade <= 0)
			{
				continue;
			}

			normalizados.Add(new CarrinhoAdicionalState(
				adicional.AdicionalId,
				adicional.Nome.Trim(),
				adicional.PrecoUnitario,
				adicional.Quantidade));
		}

		var duplicados = normalizados
			.GroupBy(adicional => adicional.AdicionalId)
			.Where(group => group.Count() > 1)
			.ToList();

		if (duplicados.Count > 0)
		{
			throw new InvalidOperationException("O mesmo adicional nao pode ser informado mais de uma vez no carrinho.");
		}

		return normalizados;
	}
}