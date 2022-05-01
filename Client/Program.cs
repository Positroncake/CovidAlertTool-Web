using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CovidAlertTool.Client;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI4MjQ0QDMyMzAyZTMxMmUzMGsxazZIU0lXdm5DY2FYajNFWkdKTVB3SXpIN1cySTdPbldvaTdUeEtOc1U9");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddGeolocationServices();
builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();
