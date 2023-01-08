using System.ComponentModel.DataAnnotations;

namespace QaDashboardApi.Entities
{
    public class Env
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
