using ComandaAi.Web.Application.Models;

namespace ComandaAi.Web.Application.Contracts;

public interface ICarrinhoPersistenciaLocalService
{
	Task<IReadOnlyList<CarrinhoItemState>> CarregarAsync(CancellationToken cancellationToken = default);

	Task SalvarAsync(IReadOnlyList<CarrinhoItemState> itens, CancellationToken cancellationToken = default);
}