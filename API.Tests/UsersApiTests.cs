using System.Net;
using System.Net.Http.Json;
using API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

namespace API.Tests;

public class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"dating-app-test-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", $"Data Source={_dbPath}");
        builder.UseSetting("Logging:LogLevel:Default", "Warning");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        SqliteConnection.ClearAllPools();
        if (File.Exists(_dbPath)) File.Delete(_dbPath);
    }
}

public class UsersApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetUsers_ReturnsSeededUsers()
    {
        var users = await factory.CreateClient().GetFromJsonAsync<List<AppUser>>("/api/users", Ct);
        Assert.Equal(["bob", "tom", "jane"], users!.Select(u => u.UserName));
    }

    [Fact]
    public async Task GetUser_ReturnsUser()
    {
        var user = await factory.CreateClient().GetFromJsonAsync<AppUser>("/api/users/2", Ct);
        Assert.Equal("tom", user!.UserName);
    }

    [Fact]
    public async Task GetUser_Missing_Returns404()
    {
        var response = await factory.CreateClient().GetAsync("/api/users/999", Ct);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Cors_AllowsAngularDevServer()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/users");
        request.Headers.Add("Origin", "http://localhost:4200");
        var response = await factory.CreateClient().SendAsync(request, Ct);
        Assert.Equal("http://localhost:4200", response.Headers.GetValues("Access-Control-Allow-Origin").Single());
    }
}
