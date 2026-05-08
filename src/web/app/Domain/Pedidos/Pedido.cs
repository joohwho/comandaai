namespace ComandaAi.Web.Domain.Pedidos;

public sealed class Pedido
{
	private readonly List<ItemPedido> itens = [];

	public Pedido(string numeroExibicao, IdentificacaoCliente cliente, ReferenciaAtendimento referenciaAtendimento)
	{
		if (string.IsNullOrWhiteSpace(numeroExibicao))
		{
			throw new ArgumentException("O numero do pedido deve ser informado.", nameof(numeroExibicao));
		}

		NumeroExibicao = numeroExibicao.Trim();
		Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
		ReferenciaAtendimento = referenciaAtendimento ?? throw new ArgumentNullException(nameof(referenciaAtendimento));
		Status = PedidoStatus.Recebido;
		CriadoEmUtc = DateTimeOffset.UtcNow;
		AtualizadoEmUtc = CriadoEmUtc;
	}

	public string NumeroExibicao { get; }

	public IdentificacaoCliente Cliente { get; }

	public ReferenciaAtendimento ReferenciaAtendimento { get; }

	public PedidoStatus Status { get; private set; }

	public DateTimeOffset CriadoEmUtc { get; }

	public DateTimeOffset AtualizadoEmUtc { get; private set; }

	public IReadOnlyList<ItemPedido> Itens => itens;

	public decimal Total => itens.Sum(item => item.Total);

	public void AdicionarItem(ItemPedido item)
	{
		ArgumentNullException.ThrowIfNull(item);

		if (Status is PedidoStatus.Entregue or PedidoStatus.Cancelado)
		{
			throw new InvalidOperationException("Nao e possivel alterar itens de um pedido encerrado.");
		}

		itens.Add(item);
		AtualizadoEmUtc = DateTimeOffset.UtcNow;
	}

	public void AtualizarStatus(PedidoStatus novoStatus)
	{
		if (!TransicaoPermitida(Status, novoStatus))
		{
			throw new InvalidOperationException($"A transicao de {Status} para {novoStatus} nao e permitida.");
		}

		Status = novoStatus;
		AtualizadoEmUtc = DateTimeOffset.UtcNow;
	}

	private static bool TransicaoPermitida(PedidoStatus atual, PedidoStatus novo)
	{
		if (atual == novo)
		{
			return true;
		}

		return atual switch
		{
			PedidoStatus.Recebido => novo is PedidoStatus.EmPreparo or PedidoStatus.Cancelado,
			PedidoStatus.EmPreparo => novo is PedidoStatus.Pronto or PedidoStatus.Cancelado,
			PedidoStatus.Pronto => novo is PedidoStatus.Entregue,
			PedidoStatus.Entregue => false,
			PedidoStatus.Cancelado => false,
			_ => false
		};
	}
}