using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AspNetAndBlazorNet7Demos.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			builder.RootComponents.Add<App>("app");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddHxServices();
			builder.Services.AddHxMessenger();
			builder.Services.AddHxMessageBoxHost();

			await builder.Build().RunAsync();
		}
	}
}