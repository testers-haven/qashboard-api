
namespace QaDashboardApi.Controllers.Responses;

public class TriggerExecutionResponse {
    public int number { get; set; }
    public string? state { get; set; }
    public string? id { get; set; }
    public string? created_at { get; set; }
}