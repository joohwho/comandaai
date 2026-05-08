using ComandaAi.Web.Application.Contracts;
using ComandaAi.Web.Application.Models;
using ComandaAi.Web.Domain.Cardapio;
using ComandaAi.Web.Domain.Pedidos;

namespace ComandaAi.Web.Application.Services;

public sealed class MontagemPedidoService(ICatalogoConsultaService catalogoConsultaService) : IMontagemPedidoService
{
	public async Task<Pedido> MontarAsync(MontagemPedidoEntrada entrada, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entrada);

		if (entrada.Itens.Count == 0)
		{
			throw new InvalidOperationException("O pedido deve conter ao menos um item.");
		}

		var categorias = await catalogoConsultaService.ListarCategoriasAsync(cancellationToken);
		var produtosPorId = categorias
			.SelectMany(categoria => categoria.Produtos)
			.ToDictionary(produto => produto.Id);

		var pedido = new Pedido(
			entrada.NumeroExibicao,
			new IdentificacaoCliente(entrada.NomeCliente),
			new ReferenciaAtendimento(entrada.Mesa, entrada.Comanda, entrada.OrigemQrCode));

		foreach (var itemEntrada in entrada.Itens)
		{
			pedido.AdicionarItem(CriarItemPedido(itemEntrada, produtosPorId));
		}

		return pedido;
	}

	private static ItemPedido CriarItemPedido(
		MontagemPedidoItemEntrada itemEntrada,
		IReadOnlyDictionary<Guid, ProdutoCardapio> produtosPorId)
	{
		if (!produtosPorId.TryGetValue(itemEntrada.ProdutoId, out var produto) || !produto.Ativo)
		{
			throw new InvalidOperationException("O produto informado nao esta disponivel para pedido.");
		}

		var adicionais = MapearAdicionais(itemEntrada, produto);

		return new ItemPedido(
			produto.Id,
			produto.Nome,
			produto.PrecoBase,
			itemEntrada.Quantidade,
			itemEntrada.Observacao,
			adicionais);
	}

	private static IReadOnlyList<ItemPedidoAdicional> MapearAdicionais(
		MontagemPedidoItemEntrada itemEntrada,
		ProdutoCardapio produto)
	{
		var adicionaisSelecionados = itemEntrada.Adicionais ?? [];
		var adicionaisDuplicados = adicionaisSelecionados
			.GroupBy(adicional => adicional.AdicionalId)
			.Where(group => group.Count() > 1)
			.Select(group => group.Key)
			.ToHashSet();

		if (adicionaisDuplicados.Count > 0)
		{
			throw new InvalidOperationException("O mesmo adicional nao pode ser informado mais de uma vez no item.");
		}

		var adicionaisDisponiveisPorId = produto.Adicionais.ToDictionary(adicional => adicional.Id);
		var adicionais = new List<ItemPedidoAdicional>(adicionaisSelecionados.Count);

		foreach (var adicionalSelecionado in adicionaisSelecionados)
		{
			if (!adicionaisDisponiveisPorId.TryGetValue(adicionalSelecionado.AdicionalId, out var adicionalDisponivel))
			{
				throw new InvalidOperationException("O adicional informado nao pertence ao produto selecionado.");
			}

			if (adicionalSelecionado.Quantidade > adicionalDisponivel.QuantidadeMaximaPorItem)
			{
				throw new InvalidOperationException("A quantidade do adicional excede o limite permitido por item.");
			}

			adicionais.Add(new ItemPedidoAdicional(
				adicionalDisponivel.Id,
				adicionalDisponivel.Nome,
				adicionalDisponivel.Preco,
				adicionalSelecionado.Quantidade));
		}

		foreach (var adicionalObrigatorio in produto.Adicionais.Where(adicional => adicional.Obrigatorio))
		{
			var foiSelecionado = adicionais.Any(adicional => adicional.AdicionalId == adicionalObrigatorio.Id);

			if (!foiSelecionado)
			{
				throw new InvalidOperationException("Existem adicionais obrigatorios nao informados para o produto.");
			}
		}

		return adicionais;
	}
}