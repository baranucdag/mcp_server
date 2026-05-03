using System.ComponentModel;
using ModelContextProtocol.Server;
using MyMcpServer.Services;

namespace MyMcpServer.Tools;

[McpServerToolType]
public class GitHubTools
{
    private readonly GitHubService _github;

    public GitHubTools(GitHubService github) => _github = github;

    [McpServerTool, Description("GitHub'daki tüm repolarını listeler")]
    public async Task<string> ListRepositories()
    {
        return await _github.GetRepositoriesAsync();
    }

    [McpServerTool, Description("Belirli bir reponun detaylarını getirir")]
    public async Task<string> GetRepository(
        [Description("Repo sahibinin kullanıcı adı")] string owner,
        [Description("Repo adı")] string repo)
    {
        return await _github.GetRepositoryAsync(owner, repo);
    }

    [McpServerTool, Description("Repodaki bir dosyanın içeriğini getirir")]
    public async Task<string> GetFileContent(
        [Description("Repo sahibinin kullanıcı adı")] string owner,
        [Description("Repo adı")] string repo,
        [Description("Dosya yolu (örn: src/Program.cs)")] string path)
    {
        return await _github.GetFileContentAsync(owner, repo, path);
    }

    [McpServerTool, Description("Repodaki issue'ları listeler")]
    public async Task<string> GetIssues(
        [Description("Repo sahibinin kullanıcı adı")] string owner,
        [Description("Repo adı")] string repo)
    {
        return await _github.GetIssuesAsync(owner, repo);
    }
}