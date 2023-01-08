using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QaDashboardApi.Clients;
using QaDashboardApi.Clients.Requests;
using QaDashboardApi.Controllers.Requests;
using QaDashboardApi.Controllers.Responses;
using TestRail;
using TestRail.Types;

namespace QaDashboardApi.Controllers;

[ApiController]
[Route("execution/")]
public class ExecutionController : ControllerBase
{
    private readonly IConfiguration Configuration;
    private readonly ILogger<ExecutionController> _logger;
    private CircleciClient circleciClient;
    private TestRailClient testRailClient;

    public ExecutionController(ILogger<ExecutionController> logger, IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
        testRailClient = new TestRailClient(Configuration["TESTRAIL_HOST"], Configuration["TESTRAIL_USERNAME"], Configuration["TESTRAIL_PASSWORD"]);
        circleciClient = new CircleciClient(Configuration["CIRCLECI_HOST"], "Circle-Token", Configuration["CIRCLECI_TOKEN"]);
    }

    [HttpPost]
    [Route("trigger")]
    public TriggerExecutionResponse Trigger(Execution execution)
    {
        var project = testRailClient.GetProject(execution.Project).Payload;

        var suiteNames = new List<string>();
        ulong planId = 0;

        if (execution.TestrailEnabled == true)
        {
            var plan = GenerateTestRailPlan(execution);
            suiteNames.AddRange(plan.Entries.Select(entry => entry.Name).ToList());
            planId = plan.Id;
        } else
        {
            suiteNames.AddRange(execution.Suites.ToList().Select(entry => entry.Name).ToList());
        }
        
        string joinSuites =  concatenateSuites(suiteNames);
        var requestBody = new PostPipelineRequest()
        {
            branch = execution.Branch,
            parameters = new PipelineParameters()
            {
                suitesFilter = joinSuites,
                project = project.Name,
                testrailReport = execution.TestrailEnabled,
                testrailProjectId = project.Id,
                testrailPlanId = planId
            }
        };

        var responseContent = circleciClient.PostPipeline(requestBody, execution.Repository).Content;

        

        return JsonSerializer.Deserialize<TriggerExecutionResponse>(responseContent);
    }

    private string concatenateSuites(List<string> suites) {
        var suiteNames = new List<string>();
        suites.ForEach(suite => suiteNames.Add(" --suite " + suite.Replace(" ", "_")));
        return string.Join("", suiteNames);
    }

    private Plan GenerateTestRailPlan(Execution execution)
    {
        List<PlanEntry> planEntries = new List<PlanEntry>();
        if (execution.Plan == 0)
        {
            if(execution.Suites.Length == 0)
            {
                testRailClient.GetSuites(execution.Project).Payload.ToList().ForEach( suite =>
                {
                    planEntries.Add(LoadSuitesToEntries(suite.Name, (ulong)suite.Id));
                });
            } else
            {
                execution.Suites.ToList().ForEach(suite =>
                {
                    planEntries.Add(LoadSuitesToEntries(suite.Name, suite.Id));
                });
            }

            return testRailClient.AddPlan(execution.Project, execution.PlanName, null, null, planEntries).Payload;
        }
        else
        {
            var plan = testRailClient.GetPlan(execution.Plan).Payload;
            var existingEntries = plan.Entries;

            var missingSuites = execution.Suites.ToList().Where( suite => !existingEntries.Any(entry => entry.Name == suite.Name));
            missingSuites.ToList().ForEach(suite =>
            {
                plan.Entries.Add(testRailClient.AddPlanEntry(plan.Id, suite.Id, suite.Name).Payload);
            });

            return plan;
        }
    }

    private PlanEntry LoadSuitesToEntries(string suiteName, ulong suiteId)
    {
        return new PlanEntry()
        {
            Name = suiteName,
            SuiteId = suiteId,
            RunList = new List<Run>() {
                new Run() {
                    IncludeAll = true,
                    SuiteId = suiteId,
                    Name = suiteName,
                }
            },
        };
    }

}
