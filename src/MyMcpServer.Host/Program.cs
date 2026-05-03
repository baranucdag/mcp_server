using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyMcpServer.Services;
using MyMcpServer.Tools;

var builder = Host.CreateApplicationBuilder(args);

// Logları stderr'e yönlendir (stdout sadece MCP için)
builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

// MCP Server'ı stdio transport ile başlat
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(FileSystemTools).Assembly);

// Servisleri DI'ya ekle
builder.Services.AddSingleton<FileSystemService>();
builder.Services.AddSingleton<GitHubService>();

var app = builder.Build();
await app.RunAsync();