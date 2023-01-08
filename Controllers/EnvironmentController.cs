using QaDashboardApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QaDashboardApi.Controllers
{
	[ApiController]
	[Route("api/environment")]
	public class EnvironmentController : Controller
    {
		private readonly QashboardContext db;

		public EnvironmentController(QashboardContext db)
		{
			this.db = db;
		}

		[HttpPut("{id}")]
		public async Task<Env> Put(Guid id, [FromBody] Env cust)
		{
			var edit = await db.Environments.FindAsync(id);
			if (edit != null)
			{
				edit.Name = cust.Name;
				await db.SaveChangesAsync();
			}
			return edit;
		}

		[HttpDelete("{id}")]
		public async Task<Env> Delete(Guid id)
		{
			var delete = await db.Environments.FindAsync(id);
			if (delete != null)
			{
				db.Environments.Remove(delete);
				await db.SaveChangesAsync();
			}
			return delete;
		}

		[HttpPost]
		public async Task<ActionResult<Env>> Post([FromBody] Env create)
		{
			var isExisting = db.Environments.Where(x => x.Name.Equals(create.Name)).Count() >= 1;
			if (isExisting)
            {
				return StatusCode(409, $"User '{create.Name}' already exists.");
			} else
            {
				create.ID = Guid.NewGuid();
				EntityEntry<Env> env = await db.Environments.AddAsync(create);
				await db.SaveChangesAsync();
				return Created("", env.Entity);
			}
		}

		[HttpGet]
		public async Task<IEnumerable<Env>> Get(string? name)
		{
			return await Task.Factory.StartNew<IEnumerable<Env>>(() =>
			{
				if (string.IsNullOrEmpty(name))
					return db.Environments;
				else
					return db.Environments.Where(x => x.Name.Contains(name));
			});
		}

		[HttpGet("{id}")]
		public async Task<Env> Get(Guid id)
		{
			return await db.Environments.FindAsync(id);
		}
	}
}
