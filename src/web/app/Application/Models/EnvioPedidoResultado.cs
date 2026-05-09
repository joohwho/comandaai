using ComandaAi.Web.Domain.Pedidos;

namespace ComandaAi.Web.Application.Models;

public sealed record EnvioPedidoResultado(
	string NumeroPedido,
	string ProtocoloOperacional,
	PedidoStatus Status,
	DateTimeOffset RecebidoEmUtc,
	string Mensagem);