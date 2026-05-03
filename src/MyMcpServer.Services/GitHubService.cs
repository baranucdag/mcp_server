using Octokit;

namespace MyMcpServer.Services;

public class GitHubService
{
    private readonly GitHubClient _client;
    private readonly string _username;

    public GitHubService()
    {
        var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        _username = Environment.GetEnvironmentVariable("GITHUB_USERNAME") ?? "";

        _client = new GitHubClient(new ProductHeaderValue("MyMcpServer"));

        if (!string.IsNullOrEmpty(token))
            _client.Credentials = new Credentials(token);
    }

    public async Task<string> GetRepositoriesAsync()
    {
        var repos = await _client.Repository.GetAllForCurrent();
        var result = repos.Select(r => new
        {
            r.Name,
            r.Description,
            r.Language,
            r.StargazersCount,
            r.HtmlUrl
        });
        return System.Text.Json.JsonSerializer.Serialize(result);
    }

    public async Task<string> GetRepositoryAsync(string owner, string repo)
    {
        var repository = await _client.Repository.Get(owner, repo);
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            repository.Name,
            repository.Description,
            repository.Language,
            repository.StargazersCount,
            repository.HtmlUrl,
            repository.DefaultBranch
        });
    }

    public async Task<string> GetFileContentAsync(string owner, string repo, string path)
    {
        var contents = await _client.Repository.Content.GetAllContents(owner, repo, path);
        return contents.FirstOrDefault()?.Content ?? "Dosya bulunamadı.";
    }

    public async Task<string> GetIssuesAsync(string owner, string repo)
    {
        var issues = await _client.Issue.GetAllForRepository(owner, repo);
        var result = issues.Select(i => new
        {
            i.Number,
            i.Title,
            i.State,
            i.HtmlUrl
        });
        return System.Text.Json.JsonSerializer.Serialize(result);
    }
}