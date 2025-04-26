using BeaniesUtilities.Models.Resume;
using FluentAssertions;
using Gay.TCazier.Resume.API;
using Gay.TCazier.Resume.API.Endpoints.V1.Create;
using Gay.TCazier.Resume.API.Endpoints.V1.Delete;
using Gay.TCazier.Resume.API.Endpoints.V1.Get;
using Gay.TCazier.Resume.API.Endpoints.V1.Put;
using Gay.TCazier.Resume.API.Mappings.V1;
using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;
using Gay.TCazier.Resume.Contracts.Endpoints.V1;
using Resume.API.Tests.Integration.Mappings.V1;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using BeaniesUtilities.Models.CommonModels;

namespace Resume.API.Tests.Integration.Endpoint.V1.Put;

public class OrganizationModelPutEndpointsTests : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<OrganizationModelResponse> _createdOrganizationModels = new List<OrganizationModelResponse>();

    public OrganizationModelPutEndpointsTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdOrganizationModels.Select(x=>x.Id))
        {
            await httpClient.DeleteAsync($"{OrganizationModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task UpdateOrganizationModel_UpdatesModel_WhenDataIsCorrect()
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
        UpdateOrganizationModelRequest updateRequest = createdResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{OrganizationModelEndpoints.EndpointPrefix}/{createdResponse.Id}", updateRequest);
        get = await httpClient.GetAsync(result.Headers.Location.AbsolutePath);
        var updatedModel = await get.Content.ReadFromJsonAsync<OrganizationModelResponse>();

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.AbsolutePath.Should().Be($"/{OrganizationModelEndpoints.EndpointPrefix}/{updatedModel.Id}");

        updatedModel.Id.Should().Be(createdResponse.Id);
        updatedModel.Name.Should().NotBe(createdResponse.Name);

		updatedModel.Industry.Should().Be(createdResponse.Industry);
		updatedModel.Status.Should().Be(createdResponse.Status);
		updatedModel.Addresses.Should().BeEquivalentTo(createdResponse.Addresses);
		updatedModel.Website.Should().Be(createdResponse.Website);
		updatedModel.Departments.Should().BeEquivalentTo(createdResponse.Departments);
		updatedModel.Emails.Should().BeEquivalentTo(createdResponse.Emails);
		updatedModel.Socials.Should().BeEquivalentTo(createdResponse.Socials);
		updatedModel.PhoneNumbers.Should().BeEquivalentTo(createdResponse.PhoneNumbers);
    }

    //public async Task UpdateOrganizationModel_DoesNotUpdateModel_WhenDataIsIncorrect()
    //{
    //    // ARRANGE
    //    var httpClient = _factory.CreateClient();
    //    //uhasdfgohjaoidfj
    //    var modelRequest = ModelGenerator.GenerateNewCreateOrganizationModelRequest();

    //    var create = await httpClient.PostAsJsonAsync(OrganizationModelEndpoints.Post, modelRequest);
    //    var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
    //    var createdResponse = await create.Content.ReadFromJsonAsync<OrganizationModelResponse>();
    //    _createdOrganizationModels.Add(createdResponse);

    //    // ACT
    //    UpdateOrganizationModelRequest updateRequest = ModelGenerator.GenerateNewUpdateOrganizationModelRequest(createdResponse);
    //    var result = await httpClient.GetAsync($"{OrganizationModelEndpoints.EndpointPrefix}/{createdResponse.Id}");
    //    var updatedModel = await result.Content.ReadFromJsonAsync<OrganizationModelResponse>();

    //    // ASSERT
    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    updatedModel.Should().BeEquivalentTo(castModel);
    //    result.Headers.Location.Should().Be($"{OrganizationModelEndpoints.EndpointPrefix}/{updatedModel.Id}");
    //}

    [Fact]
    public async Task UpdateOrganizationModel_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var propRecord = await ModelGenerator.PopulateDatabaseForOrganizationModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateOrganizationModelRequest(propRecord);
        var fakedModel = modelRequest.MapToModelFromCreateRequest(-10000,"IntegrationTesting", new List<AddressModel>(), new List<PhoneNumberModel>());
        var fakedResponse = fakedModel.MapToResponseFromModel();

        // ACT
        UpdateOrganizationModelRequest updateRequest = fakedResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{OrganizationModelEndpoints.EndpointPrefix}/{updateRequest.Id}", updateRequest);

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
