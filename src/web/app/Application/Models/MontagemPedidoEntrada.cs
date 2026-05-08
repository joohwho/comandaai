namespace ComandaAi.Web.Application.Models;

public sealed record MontagemPedidoEntrada(
	string NumeroExibicao,
	string NomeCliente,
	string? Mesa,
	string? Comanda,
	string? OrigemQrCode,
	IReadOnlyList<MontagemPedidoItemEntrada> Itens);

public sealed record MontagemPedidoItemEntrada(
	Guid ProdutoId,
	int Quantidade,
	string? Observacao = null,
	IReadOnlyList<MontagemPedidoAdicionalEntrada>? Adicionais = null);

public sealed record MontagemPedidoAdicionalEntrada(Guid AdicionalId, int Quantidade);