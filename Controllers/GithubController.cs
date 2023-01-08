using Microsoft.AspNetCore.Mvc;
using Octokit;
using QaDashboardApi.Controllers.Responses;

namespace QaDashboardApi.Controllers;

[ApiController]
[Route("github/")]
public class GithubController : ControllerBase
{
    private readonly IConfiguration Configuration;
    private GitHubClient gitHubClient;

    private readonly ILogger<GithubController> _logger;

    public GithubController(ILogger<GithubController> logger, IConfiguration configuration)
    {
        _logger = logger;
                  Configuration = configuration;
            gitHubClient = new GitHubClient(new ProductHeaderValue("tests-executor"));
            var tokenAuth = new Credentials(Configuration["GITHUB_TOKEN"], AuthenticationType.Oauth);
            gitHubClient.Credentials = tokenAuth;
    }

    [HttpGet]
    [Route("branches")]
    public IEnumerable<BranchResponse> Branches(string repositoryName)
    {
        var branches = gitHubClient.Repository.Branch.GetAll("coingaming", repositoryName).GetAwaiter().GetResult();
        
        var response = branches.Select(p =>
                        new BranchResponse
                        {
                            name = p.Name,
                            label = p.Name
                        }).ToArray();

        return response;
    }
}
