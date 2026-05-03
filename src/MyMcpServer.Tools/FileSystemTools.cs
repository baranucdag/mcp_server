using System.ComponentModel;
using ModelContextProtocol.Server;
using MyMcpServer.Services;

namespace MyMcpServer.Tools;

[McpServerToolType]
public class FileSystemTools

{
    private readonly FileSystemService _fs;

    public FileSystemTools(FileSystemService fs) => _fs = fs;

    [McpServerTool, Description("Belirtilen dosyanın içeriğini okur")]
    public async Task<string> ReadFile(
        [Description("Root'a göre relative dosya yolu (örn: src/Program.cs)")] 
        string path)
    {
        return await _fs.ReadFileAsync(path);
    }

    [McpServerTool, Description("Dizindeki tüm dosyaları listeler")]
    public async Task<string> ListFiles(
        [Description("Listelenecek dizin (boş bırakılırsa root)")] 
        string directory = "")
    {
        var files = await _fs.ListFilesAsync(directory);
        return string.Join("\n", files);
    }

    [McpServerTool, Description("Dizinleri listeler")]
    public async Task<string> ListDirectories(
        [Description("Listelenecek dizin (boş bırakılırsa root)")] 
        string directory = "")
    {
        var dirs = await _fs.ListDirectoriesAsync(directory);
        return string.Join("\n", dirs);
    }
}