using System.ComponentModel.DataAnnotations;

namespace QaDashboardApi.Entities
{
	public class Project
    {
		public Guid id { get; set; }
		public int[] RepositoriesIds { get; set; }

    }
}
