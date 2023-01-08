using QaDashboardApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QaDashboardApi.Controllers
{
	[ApiController]
	[Route("api/platform")]
	public class PlatformController : Controller
    {
		private readonly QashboardContext db;

		public PlatformController(QashboardContext db)
		{
			this.db = db;
		}

		[HttpPut("{id}")]
		public async Task<Platform> Put(Guid id, [FromBody] Env cust)
		{
			var edit = await db.Platforms.FindAsync(id);
			if (edit != null)
			{
				edit.Name = cust.Name;
				await db.SaveChangesAsync();
			}
			return edit;
		}

		[HttpDelete("{id}")]
		public async Task<Platform> Delete(Guid id)
		{
			var delete = await db.Platforms.FindAsync(id);
			if (delete != null)
			{
				db.Platforms.Remove(delete);
				await db.SaveChangesAsync();
			}
			return delete;
		}

		[HttpPost]
		public async Task<ActionResult<Platform>> Post([FromBody] Platform create)
		{
			var isExisting = db.Platforms.Where(x => x.Name.Equals(create.Name)).Count() >= 1;
			if (isExisting)
            {
				return StatusCode(409, $"User '{create.Name}' already exists.");
			} else
            {
				create.ID = Guid.NewGuid();
				EntityEntry<Platform> env = await db.Platforms.AddAsync(create);
				await db.SaveChangesAsync();
				return Created("", env.Entity);
			}
		}

		[HttpGet]
		public async Task<IEnumerable<Platform>> Get(string? name)
		{
			return await Task.Factory.StartNew<IEnumerable<Platform>>(() =>
			{
				if (string.IsNullOrEmpty(name))
					return db.Platforms;
				else
					return db.Platforms.Where(x => x.Name.Contains(name));
			});
		}

		[HttpGet("{id}")]
		public async Task<Platform> Get(Guid id)
		{
			return await db.Platforms.FindAsync(id);
		}
	}
}
