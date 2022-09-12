namespace AspNetAndBlazorNet7Demos
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			builder.Services.AddOutputCache();

			var app = builder.Build();


			if (app.Environment.IsDevelopment())
			{
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();


			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseOutputCache();

			app.MapRazorPages();
			app.MapControllers();

			app.MapGet("/outputcache/minimal-api", () => DateTime.Now.ToString())
				.CacheOutput(p => p.Expire(TimeSpan.FromSeconds(5))); // MinimalAPI caching

			app.MapFallbackToPage("/_Host");

			app.Run();
		}
	}
}