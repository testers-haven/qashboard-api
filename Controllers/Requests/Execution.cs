
namespace QaDashboardApi.Controllers.Requests;

public class Execution {
        public ulong Project { get; set; }
        public string? Environment { get; set; }
        public string? Platform { get; set; }
        public Suites[]? Suites { get; set; }
        public bool TestrailEnabled { get; set; }
        public string? Repository { get; set; }
        public string? Branch { get; set; }
        public ulong Plan { get; set; }
        public string? PlanName { get; set; }
}