namespace QaDashboardApi.Entities
{
    public class Repository
    {
        public Guid Id { get; set; }
        public int PlatformId { get; set; }
        public string CiUrl { get; set; }
        public string GitUrl { get; set; }
        public int[] envIds { get; set; }
        public int XrayProjectId { get; set; }
        public int TestrailProjectId { get; set; }
        public int AllureProjectId { get; set; }

    }
}
