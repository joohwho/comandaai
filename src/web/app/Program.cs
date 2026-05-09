using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComandaAi.Web.Application.Contracts;
using ComandaAi.Web.Application.Services;
using ComandaAi.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ICarrinhoPedidoService, CarrinhoPedidoService>();
builder.Services.AddScoped<ICarrinhoPersistenciaLocalService, CarrinhoPersistenciaLocalService>();
builder.Services.AddScoped<ICatalogoConsultaService, CatalogoConsultaService>();
builder.Services.AddScoped<IEnvioPedidoService, EnvioPedidoLocalService>();
builder.Services.AddScoped<IMontagemPedidoService, MontagemPedidoService>();

await builder.Build().RunAsync();
