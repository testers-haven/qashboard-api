namespace QaDashboardApi.Clients.Requests;
public class PostPipelineRequest
{
    public string? branch;
    public PipelineParameters? parameters;
}