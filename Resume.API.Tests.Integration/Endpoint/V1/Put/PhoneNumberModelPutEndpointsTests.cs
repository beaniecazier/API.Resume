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

public class PhoneNumberModelPutEndpointsTests : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<PhoneNumberModelResponse> _createdPhoneNumberModels = new List<PhoneNumberModelResponse>();

    public PhoneNumberModelPutEndpointsTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdPhoneNumberModels.Select(x=>x.Id))
        {
            await httpClient.DeleteAsync($"{PhoneNumberModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task UpdatePhoneNumberModel_UpdatesModel_WhenDataIsCorrect()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var propRecord = await ModelGenerator.PopulateDatabaseForPhoneNumberModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreatePhoneNumberModelRequest(propRecord);

        var create = await httpClient.PostAsJsonAsync(PhoneNumberModelEndpoints.Post, modelRequest);
        var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
        var createdResponse = await get.Content.ReadFromJsonAsync<PhoneNumberModelResponse>();
        _createdPhoneNumberModels.Add(createdResponse);

        // ACT
        UpdatePhoneNumberModelRequest updateRequest = createdResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{PhoneNumberModelEndpoints.EndpointPrefix}/{createdResponse.Id}", updateRequest);
        get = await httpClient.GetAsync(result.Headers.Location.AbsolutePath);
        var updatedModel = await get.Content.ReadFromJsonAsync<PhoneNumberModelResponse>();

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.AbsolutePath.Should().Be($"/{PhoneNumberModelEndpoints.EndpointPrefix}/{updatedModel.Id}");

        updatedModel.Id.Should().Be(createdResponse.Id);
        updatedModel.Name.Should().NotBe(createdResponse.Name);

		updatedModel.CountryCode.Should().Be(createdResponse.CountryCode.Id);
		updatedModel.AreaCode.Should().Be(createdResponse.AreaCode);
		updatedModel.TelephonePrefix.Should().Be(createdResponse.TelephonePrefix);
		updatedModel.LineNumber.Should().Be(createdResponse.LineNumber);
		updatedModel.PhoneType.Should().Be(createdResponse.PhoneType);
    }

    //public async Task UpdatePhoneNumberModel_DoesNotUpdateModel_WhenDataIsIncorrect()
    //{
    //    // ARRANGE
    //    var httpClient = _factory.CreateClient();
    //    //uhasdfgohjaoidfj
    //    var modelRequest = ModelGenerator.GenerateNewCreatePhoneNumberModelRequest();

    //    var create = await httpClient.PostAsJsonAsync(PhoneNumberModelEndpoints.Post, modelRequest);
    //    var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
    //    var createdResponse = await create.Content.ReadFromJsonAsync<PhoneNumberModelResponse>();
    //    _createdPhoneNumberModels.Add(createdResponse);

    //    // ACT
    //    UpdatePhoneNumberModelRequest updateRequest = ModelGenerator.GenerateNewUpdatePhoneNumberModelRequest(createdResponse);
    //    var result = await httpClient.GetAsync($"{PhoneNumberModelEndpoints.EndpointPrefix}/{createdResponse.Id}");
    //    var updatedModel = await result.Content.ReadFromJsonAsync<PhoneNumberModelResponse>();

    //    // ASSERT
    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    updatedModel.Should().BeEquivalentTo(castModel);
    //    result.Headers.Location.Should().Be($"{PhoneNumberModelEndpoints.EndpointPrefix}/{updatedModel.Id}");
    //}

    [Fact]
    public async Task UpdatePhoneNumberModel_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var propRecord = await ModelGenerator.PopulateDatabaseForPhoneNumberModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreatePhoneNumberModelRequest(propRecord);
        var fakedModel = modelRequest.MapToModelFromCreateRequest(-10000,"IntegrationTesting", new CountryCodeModel());
        var fakedResponse = fakedModel.MapToResponseFromModel();

        // ACT
        UpdatePhoneNumberModelRequest updateRequest = fakedResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{PhoneNumberModelEndpoints.EndpointPrefix}/{updateRequest.Id}", updateRequest);

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
