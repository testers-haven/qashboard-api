using System.ComponentModel.DataAnnotations;

namespace QaDashboardApi.Entities
{
    public class Reporter
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string ApiUrl { get; set; }
    }
}
