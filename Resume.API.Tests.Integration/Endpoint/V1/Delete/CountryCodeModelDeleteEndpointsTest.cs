using Gay.TCazier.Resume.API.Endpoints.V1.Delete;
using Gay.TCazier.Resume.API;
using Gay.TCazier.Resume.Contracts.Responses.V1;
using Microsoft.AspNetCore.Mvc.Testing;
using Gay.TCazier.Resume.API.Endpoints.V1.Get;
using System.Net;
using FluentAssertions;
using Gay.TCazier.Resume.API.Endpoints.V1.Create;
using Gay.TCazier.Resume.Contracts.Endpoints.V1;

namespace Resume.API.Tests.Integration.Endpoint.V1.Delete;

public class CountryCodeModelDeleteEndpointsTest : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<CountryCodeModelResponse> _createdCountryCodeModels = new List<CountryCodeModelResponse>();

    public CountryCodeModelDeleteEndpointsTest(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdCountryCodeModels.Select(x => x.Id))
        {
            await httpClient.DeleteAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task DeleteCountryCodeModel_ReturnsOk_WhenModelDoesExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();

        var propRecord = await ModelGenerator.PopulateDatabaseForCountryCodeModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateCountryCodeModelRequest(propRecord);

        var create = await httpClient.PostAsJsonAsync(CountryCodeModelEndpoints.Post, modelRequest);
        var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
        var createdResponse = await get.Content.ReadFromJsonAsync<CountryCodeModelResponse>();
        _createdCountryCodeModels.Add(createdResponse);

        // ACT
        var result = await httpClient.DeleteAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{createdResponse.Id}");

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteCountryCodeModel_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();

        // ACT
        var result = await httpClient.DeleteAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{-1000000}");

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
