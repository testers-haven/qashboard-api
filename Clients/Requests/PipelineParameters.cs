namespace QaDashboardApi.Clients.Requests;
public class PipelineParameters
{
    public string? suitesFilter { get; set; }
    public string? project { get; set; }
    public bool testrailReport { get; set; }
    public ulong testrailProjectId { get; set; }
    public ulong testrailPlanId { get; set; }
}