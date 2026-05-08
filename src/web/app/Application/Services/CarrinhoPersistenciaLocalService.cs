using System.Text.Json;
using ComandaAi.Web.Application.Contracts;
using ComandaAi.Web.Application.Models;
using Microsoft.JSInterop;

namespace ComandaAi.Web.Application.Services;

public sealed class CarrinhoPersistenciaLocalService(IJSRuntime jsRuntime) : ICarrinhoPersistenciaLocalService
{
	private const string StorageKey = "comandaai.carrinho";
	private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

	public async Task<IReadOnlyList<CarrinhoItemState>> CarregarAsync(CancellationToken cancellationToken = default)
	{
		var json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", cancellationToken, StorageKey);

		if (string.IsNullOrWhiteSpace(json))
		{
			return [];
		}

		return JsonSerializer.Deserialize<IReadOnlyList<CarrinhoItemState>>(json, SerializerOptions) ?? [];
	}

	public async Task SalvarAsync(IReadOnlyList<CarrinhoItemState> itens, CancellationToken cancellationToken = default)
	{
		if (itens.Count == 0)
		{
			await jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, StorageKey);
			return;
		}

		var json = JsonSerializer.Serialize(itens, SerializerOptions);
		await jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, StorageKey, json);
	}
}