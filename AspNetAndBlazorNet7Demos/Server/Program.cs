using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

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

			app.UseRateLimiter(new RateLimiterOptions()
			{
				GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
				{
					if (!context.Request.Path.StartsWithSegments("/rate-limiter"))
					{
						return RateLimitPartition.CreateNoLimiter("UnlimitedRequests");
					}

					return RateLimitPartition.CreateFixedWindowLimiter("GeneralLimit",
						_ => new FixedWindowRateLimiterOptions(
							permitLimit: 2,
							queueProcessingOrder: QueueProcessingOrder.OldestFirst,
							queueLimit: 10,  // try changing the limit to 0
							window: TimeSpan.FromSeconds(5)));
				}),
				RejectionStatusCode = 429 // Too Many Requests
			});

			app.UseOutputCache();

			app.MapRazorPages();
			app.MapControllers();

			app.MapGet("/outputcache/minimal-api", () => DateTime.Now.ToString())
				.CacheOutput(p => p.Expire(TimeSpan.FromSeconds(5))); // MinimalAPI caching

			app.MapGet("/rate-limiter", () => DateTime.Now.ToString());

			app.MapFallbackToPage("/_Host");

			app.Run();
		}
	}
}