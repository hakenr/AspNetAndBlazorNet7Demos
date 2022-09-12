using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AspNetAndBlazorNet7Demos.Server.Controllers
{
	[ApiController]
	public class OutputCacheController : ControllerBase
	{
		[OutputCache(Duration = 5)] /* 5 sec */
		[HttpGet("/outputcache/cached")]
		public Task<string> GetCached()
		{
			return GetExpensiveData();
		}

		[HttpGet("/outputcache/no-cache")]
		public Task<string> GetNoCache()
		{
			return GetExpensiveData();
		}

		private async Task<string> GetExpensiveData()
		{
			await Task.Delay(50); // 50 ms simulation of "expensive"
			return DateTime.Now.ToString();
		}
	}
}
