using ComandaAi.Web.Domain.Cardapio;

namespace ComandaAi.Web.Application.Contracts;

public interface ICatalogoConsultaService
{
	Task<IReadOnlyList<CategoriaCardapio>> ListarCategoriasAsync(CancellationToken cancellationToken = default);
}