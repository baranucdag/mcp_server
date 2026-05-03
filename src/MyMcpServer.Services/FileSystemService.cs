namespace MyMcpServer.Services;

public class FileSystemService
{
    private readonly string _rootPath;

    public FileSystemService()
    {
        _rootPath = Environment.GetEnvironmentVariable("MCP_ROOT_PATH")
            ?? Directory.GetCurrentDirectory();
    }

    public string RootPath => _rootPath;

    public async Task<string> ReadFileAsync(string relativePath)
    {
        var fullPath = ResolvePath(relativePath);

        if (!File.Exists(fullPath))
            return $"Hata: Dosya bulunamadı → {relativePath}";

        return await File.ReadAllTextAsync(fullPath);
    }

    public Task<IEnumerable<string>> ListFilesAsync(string relativeDir = "")
    {
        var fullPath = ResolvePath(relativeDir);

        if (!Directory.Exists(fullPath))
            return Task.FromResult(Enumerable.Empty<string>());

        var files = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(_rootPath, f));

        return Task.FromResult(files);
    }

    public Task<IEnumerable<string>> ListDirectoriesAsync(string relativeDir = "")
    {
        var fullPath = ResolvePath(relativeDir);

        if (!Directory.Exists(fullPath))
            return Task.FromResult(Enumerable.Empty<string>());

        var dirs = Directory.GetDirectories(fullPath, "*", SearchOption.AllDirectories)
            .Select(d => Path.GetRelativePath(_rootPath, d));

        return Task.FromResult(dirs);
    }

    private string ResolvePath(string relativePath)
    {
        var fullPath = Path.GetFullPath(
            Path.Combine(_rootPath, relativePath));

        // Path traversal koruması
        if (!fullPath.StartsWith(_rootPath))
            throw new UnauthorizedAccessException("Erişim reddedildi.");

        return fullPath;
    }
}