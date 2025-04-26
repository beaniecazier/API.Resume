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

public class ContactModelPutEndpointsTests : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<ContactModelResponse> _createdContactModels = new List<ContactModelResponse>();

    public ContactModelPutEndpointsTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdContactModels.Select(x=>x.Id))
        {
            await httpClient.DeleteAsync($"{ContactModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task UpdateContactModel_UpdatesModel_WhenDataIsCorrect()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var propRecord = await ModelGenerator.PopulateDatabaseForContactModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateContactModelRequest(propRecord);

        var create = await httpClient.PostAsJsonAsync(ContactModelEndpoints.Post, modelRequest);
        var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
        var createdResponse = await get.Content.ReadFromJsonAsync<ContactModelResponse>();
        _createdContactModels.Add(createdResponse);

        // ACT
        UpdateContactModelRequest updateRequest = createdResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{ContactModelEndpoints.EndpointPrefix}/{createdResponse.Id}", updateRequest);
        get = await httpClient.GetAsync(result.Headers.Location.AbsolutePath);
        var updatedModel = await get.Content.ReadFromJsonAsync<ContactModelResponse>();

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.AbsolutePath.Should().Be($"/{ContactModelEndpoints.EndpointPrefix}/{updatedModel.Id}");

        updatedModel.Id.Should().Be(createdResponse.Id);
        updatedModel.Name.Should().NotBe(createdResponse.Name);

		updatedModel.Emails.Should().BeEquivalentTo(createdResponse.Emails);
		updatedModel.Socials.Should().BeEquivalentTo(createdResponse.Socials);
		updatedModel.PhoneNumbers.Should().BeEquivalentTo(createdResponse.PhoneNumbers);
    }

    //public async Task UpdateContactModel_DoesNotUpdateModel_WhenDataIsIncorrect()
    //{
    //    // ARRANGE
    //    var httpClient = _factory.CreateClient();
    //    //uhasdfgohjaoidfj
    //    var modelRequest = ModelGenerator.GenerateNewCreateContactModelRequest();

    //    var create = await httpClient.PostAsJsonAsync(ContactModelEndpoints.Post, modelRequest);
    //    var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
    //    var createdResponse = await create.Content.ReadFromJsonAsync<ContactModelResponse>();
    //    _createdContactModels.Add(createdResponse);

    //    // ACT
    //    UpdateContactModelRequest updateRequest = ModelGenerator.GenerateNewUpdateContactModelRequest(createdResponse);
    //    var result = await httpClient.GetAsync($"{ContactModelEndpoints.EndpointPrefix}/{createdResponse.Id}");
    //    var updatedModel = await result.Content.ReadFromJsonAsync<ContactModelResponse>();

    //    // ASSERT
    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    updatedModel.Should().BeEquivalentTo(castModel);
    //    result.Headers.Location.Should().Be($"{ContactModelEndpoints.EndpointPrefix}/{updatedModel.Id}");
    //}

    [Fact]
    public async Task UpdateContactModel_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var propRecord = await ModelGenerator.PopulateDatabaseForContactModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateContactModelRequest(propRecord);
        var fakedModel = modelRequest.MapToModelFromCreateRequest(-10000,"IntegrationTesting", new List<PhoneNumberModel>());
        var fakedResponse = fakedModel.MapToResponseFromModel();

        // ACT
        UpdateContactModelRequest updateRequest = fakedResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{ContactModelEndpoints.EndpointPrefix}/{updateRequest.Id}", updateRequest);

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
