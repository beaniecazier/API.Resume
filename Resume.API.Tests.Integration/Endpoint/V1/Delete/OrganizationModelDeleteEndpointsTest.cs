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

public class OrganizationModelDeleteEndpointsTest : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<OrganizationModelResponse> _createdOrganizationModels = new List<OrganizationModelResponse>();

    public OrganizationModelDeleteEndpointsTest(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdOrganizationModels.Select(x => x.Id))
        {
            await httpClient.DeleteAsync($"{OrganizationModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task DeleteOrganizationModel_ReturnsOk_WhenModelDoesExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();

        var propRecord = await ModelGenerator.PopulateDatabaseForOrganizationModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateOrganizationModelRequest(propRecord);

        var create = await httpClient.PostAsJsonAsync(OrganizationModelEndpoints.Post, modelRequest);
        var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
        var createdResponse = await get.Content.ReadFromJsonAsync<OrganizationModelResponse>();
        _createdOrganizationModels.Add(createdResponse);

        // ACT
        var result = await httpClient.DeleteAsync($"{OrganizationModelEndpoints.EndpointPrefix}/{createdResponse.Id}");

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteOrganizationModel_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();

        // ACT
        var result = await httpClient.DeleteAsync($"{OrganizationModelEndpoints.EndpointPrefix}/{-1000000}");

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
