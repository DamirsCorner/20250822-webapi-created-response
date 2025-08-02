using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using WebApiCreatedResponse.Models;

namespace WebApiCreatedResponse.Tests;

public class MessageControllerTests
{
    WebApplicationFactory<Program> _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }

    [Test]
    public async Task EmptyReturns204OnSuccess()
    {
        using var httpClient = _factory.CreateClient();

        var message = new Message(Guid.NewGuid(), "foo");
        var response = await httpClient.PostAsJsonAsync("/message/empty", message);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task WithLocationReturns201OnSuccess()
    {
        using var httpClient = _factory.CreateClient();

        var message = new Message(Guid.NewGuid(), "foo");
        var response = await httpClient.PostAsJsonAsync("/message/location", message);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Test]
    public async Task WithWorkaroundReturns201OnSuccess()
    {
        using var httpClient = _factory.CreateClient();

        var message = new Message(Guid.NewGuid(), "foo");
        var response = await httpClient.PostAsJsonAsync("/message/workaround", message);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}
