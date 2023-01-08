using QaDashboardApi.Clients.Requests;
using QaDashboardApi.Controllers.Requests;
using RestSharp;

namespace QaDashboardApi.Clients;

public class CircleciClient
{
    readonly private RestClient client;

    public CircleciClient(string baseUrl, string key, string value)
    {
        var options = new RestClientOptions(baseUrl);
        client = new RestClient(options) { };
        client.AddDefaultHeader(key, value);
    }

    public RestResponse PostPipeline(PostPipelineRequest requestBody, string project)
    {
        var restRequest = new RestRequest($"/project/gh/coingaming/{project}/pipeline")
        {
            Method = Method.Post,
        };
        restRequest.AddJsonBody(requestBody);

        return client.PostAsync(restRequest).GetAwaiter().GetResult();
    }
}