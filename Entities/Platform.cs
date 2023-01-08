using System.ComponentModel.DataAnnotations;

namespace QaDashboardApi.Entities
{
    public class Platform
    {
        [Key]
        public Guid ID { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
