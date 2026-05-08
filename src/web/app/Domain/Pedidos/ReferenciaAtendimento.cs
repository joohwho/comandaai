namespace ComandaAi.Web.Domain.Pedidos;

public sealed record ReferenciaAtendimento
{
	public ReferenciaAtendimento(string? mesa, string? comanda, string? origemQrCode)
	{
		if (string.IsNullOrWhiteSpace(mesa) && string.IsNullOrWhiteSpace(comanda) && string.IsNullOrWhiteSpace(origemQrCode))
		{
			throw new ArgumentException("Pelo menos uma referencia operacional deve ser informada.");
		}

		Mesa = mesa?.Trim();
		Comanda = comanda?.Trim();
		OrigemQrCode = origemQrCode?.Trim();
	}

	public string? Mesa { get; }

	public string? Comanda { get; }

	public string? OrigemQrCode { get; }
}