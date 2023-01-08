using Microsoft.AspNetCore.Mvc;
using QaDashboardApi.Controllers.Responses;
using TestRail;

namespace QaDashboardApi.Controllers;

[ApiController]
[Route("testrail/")]
public class TestrailController : ControllerBase
{
    private readonly IConfiguration Configuration;
    private TestRailClient testRailClient;

    private readonly ILogger<TestrailController> _logger;

    public TestrailController(ILogger<TestrailController> logger, IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
        testRailClient = new TestRailClient(Configuration["TESTRAIL_HOST"], Configuration["TESTRAIL_USERNAME"], Configuration["TESTRAIL_PASSWORD"]);
    }

    [HttpGet]
    [Route("projects")]
    public IEnumerable<ProjectResponse> Projects()
    {
        var projects = testRailClient.GetProjects().Payload;

        return projects.Select(p =>
                    new ProjectResponse
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToArray();
    }

    [HttpGet]
    [Route("suites")]
    public IEnumerable<SuiteResponse> Suites(ulong projectId)
    {
        var projects = testRailClient.GetSuites(projectId).Payload;

        var suites = projects.Select(p =>
                                new SuiteResponse
                                {
                                    value = (ulong)p.Id,
                                    name = p.Name,
                                    label = p.Name
                                }).ToArray();

        return suites;
    }

    [HttpGet]
    [Route("plans")]
    public IEnumerable<PlanResponse> Plans(ulong projectId)
    {
        var plans = testRailClient.GetPlans(projectId).Payload;

        var response = plans.Select(p =>
                                new PlanResponse
                                {
                                    value = (ulong)p.Id,
                                    name = p.Name,
                                    label = p.Name
                                }).ToArray();

        return response;
    }
}
