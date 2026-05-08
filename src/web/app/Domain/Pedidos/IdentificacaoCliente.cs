namespace ComandaAi.Web.Domain.Pedidos;

public sealed record IdentificacaoCliente
{
	public IdentificacaoCliente(string nomeExibicao)
	{
		if (string.IsNullOrWhiteSpace(nomeExibicao))
		{
			throw new ArgumentException("O nome ou apelido do cliente deve ser informado.", nameof(nomeExibicao));
		}

		NomeExibicao = nomeExibicao.Trim();
	}

	public string NomeExibicao { get; }
}