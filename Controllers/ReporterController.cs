using QaDashboardApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QaDashboardApi.Controllers
{
	[ApiController]
	[Route("api/reporter")]
	public class ReporterController : Controller
    {
		private readonly QashboardContext db;

		public ReporterController(QashboardContext db)
		{
			this.db = db;
		}

		[HttpPut("{id}")]
		public async Task<Reporter> Put(Guid id, [FromBody] Reporter cust)
		{
			var edit = await db.Reporters.FindAsync(id);
			if (edit != null)
			{
				edit.Name = cust.Name;
				await db.SaveChangesAsync();
			}
			return edit;
		}

		[HttpDelete("{id}")]
		public async Task<Reporter> Delete(Guid id)
		{
			var delete = await db.Reporters.FindAsync(id);
			if (delete != null)
			{
				db.Reporters.Remove(delete);
				await db.SaveChangesAsync();
			}
			return delete;
		}

		[HttpPost]
		public async Task<ActionResult<Reporter>> Post([FromBody] Reporter create)
		{
			var isExisting = db.Reporters.Where(x => x.Name.Equals(create.Name)).Count() >= 1;
			if (isExisting)
            {
				return StatusCode(409, $"User '{create.Name}' already exists.");
			} else
            {
				create.ID = Guid.NewGuid();
				EntityEntry<Reporter> env = await db.Reporters.AddAsync(create);
				await db.SaveChangesAsync();
				return Created("", env.Entity);
			}
		}

		[HttpGet]
		public async Task<IEnumerable<Reporter>> Get(string? name)
		{
			return await Task.Factory.StartNew<IEnumerable<Reporter>>(() =>
			{
				if (string.IsNullOrEmpty(name))
					return db.Reporters;
				else
					return db.Reporters.Where(x => x.Name.Contains(name));
			});
		}

		[HttpGet("{id}")]
		public async Task<Reporter> Get(Guid id)
		{
			return await db.Reporters.FindAsync(id);
		}
	}
}
