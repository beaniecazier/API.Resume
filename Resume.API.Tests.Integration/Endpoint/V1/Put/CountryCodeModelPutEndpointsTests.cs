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

namespace Resume.API.Tests.Integration.Endpoint.V1.Put;

public class CountryCodeModelPutEndpointsTests : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<CountryCodeModelResponse> _createdCountryCodeModels = new List<CountryCodeModelResponse>();

    public CountryCodeModelPutEndpointsTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdCountryCodeModels.Select(x=>x.Id))
        {
            await httpClient.DeleteAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task UpdateCountryCodeModel_UpdatesModel_WhenDataIsCorrect()
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
        UpdateCountryCodeModelRequest updateRequest = createdResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{createdResponse.Id}", updateRequest);
        get = await httpClient.GetAsync(result.Headers.Location.AbsolutePath);
        var updatedModel = await get.Content.ReadFromJsonAsync<CountryCodeModelResponse>();

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.AbsolutePath.Should().Be($"/{CountryCodeModelEndpoints.EndpointPrefix}/{updatedModel.Id}");

        updatedModel.Id.Should().Be(createdResponse.Id);
        updatedModel.Name.Should().NotBe(createdResponse.Name);

		updatedModel.Country.Should().Be(createdResponse.Country);
		updatedModel.CallingCode.Should().Be(createdResponse.CallingCode);
		updatedModel.Iso3Letter.Should().Be(createdResponse.Iso3Letter);
		updatedModel.Iso2Letter.Should().Be(createdResponse.Iso2Letter);
		updatedModel.IsoNumeric.Should().Be(createdResponse.IsoNumeric);
    }

    //public async Task UpdateCountryCodeModel_DoesNotUpdateModel_WhenDataIsIncorrect()
    //{
    //    // ARRANGE
    //    var httpClient = _factory.CreateClient();
    //    //uhasdfgohjaoidfj
    //    var modelRequest = ModelGenerator.GenerateNewCreateCountryCodeModelRequest();

    //    var create = await httpClient.PostAsJsonAsync(CountryCodeModelEndpoints.Post, modelRequest);
    //    var get = await httpClient.GetAsync(create.Headers.Location.AbsolutePath);
    //    var createdResponse = await create.Content.ReadFromJsonAsync<CountryCodeModelResponse>();
    //    _createdCountryCodeModels.Add(createdResponse);

    //    // ACT
    //    UpdateCountryCodeModelRequest updateRequest = ModelGenerator.GenerateNewUpdateCountryCodeModelRequest(createdResponse);
    //    var result = await httpClient.GetAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{createdResponse.Id}");
    //    var updatedModel = await result.Content.ReadFromJsonAsync<CountryCodeModelResponse>();

    //    // ASSERT
    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    updatedModel.Should().BeEquivalentTo(castModel);
    //    result.Headers.Location.Should().Be($"{CountryCodeModelEndpoints.EndpointPrefix}/{updatedModel.Id}");
    //}

    [Fact]
    public async Task UpdateCountryCodeModel_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var propRecord = await ModelGenerator.PopulateDatabaseForCountryCodeModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateCountryCodeModelRequest(propRecord);
        var fakedModel = modelRequest.MapToModelFromCreateRequest(-10000,"IntegrationTesting");
        var fakedResponse = fakedModel.MapToResponseFromModel();

        // ACT
        UpdateCountryCodeModelRequest updateRequest = fakedResponse.MapToUpdateRequest();
        var result = await httpClient.PutAsJsonAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{updateRequest.Id}", updateRequest);

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
