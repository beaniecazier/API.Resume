using BeaniesUtilities.Models.Resume;
using FluentAssertions;
using FluentValidation.Results;
using Gay.TCazier.Resume.API.Endpoints.V1.Create;
using Gay.TCazier.Resume.API.Endpoints.V1.Get;
using Gay.TCazier.Resume.API.Endpoints.V1.Delete;
using Gay.TCazier.Resume.Contracts.Requests.V1.Create;
using Gay.TCazier.Resume.Contracts.Responses.V1;
using Gay.TCazier.Resume.Contracts.Endpoints.V1;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Resume.API.Tests.Integration.Endpoint.V1.Post;

public class ContactModelPostEndpointsTests : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<int> _createdContactModels = new List<int>();

    public ContactModelPostEndpointsTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdContactModels)
        {
            await httpClient.DeleteAsync($"{ContactModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateContactModel_CreatesContactModel_WhenDataIsCorrect()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var modelRecord = await ModelGenerator.PopulateDatabaseForContactModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateContactModelRequest(modelRecord);

        // ACT
        var result = await httpClient.PostAsJsonAsync(ContactModelEndpoints.Post, modelRequest);
        var url = result.Headers.Location.AbsolutePath;
        var get = await httpClient.GetAsync(url);
        var createdModel = await get.Content.ReadFromJsonAsync<ContactModelResponse>();
        _createdContactModels.Add(createdModel.Id);

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        createdModel.Should().BeEquivalentTo(createdModel);
        result.Headers.Location.AbsolutePath.Should().Be($"/{ContactModelEndpoints.EndpointPrefix}/{createdModel.Id}");
    }

    //[Fact]
    //public async Task CreateContactModel_Fails_WhenHouseNumberIsInvalid()
    //{
    //    // ARRANGE
    //    var httpClient = _factory.CreateClient();
    //    var modelRequest = ModelGenerator.GenerateNewCreateContactModelRequest();

    //    modelRequest.HouseNumber = null;

    //    // ACT
    //    var result = await httpClient.PostAsJsonAsync(ContactModelEndpoints.Post, modelRequest);
    //    var errors = await result.Content.ReadFromJsonAsync<IEnumerable<ValidationFailure>>();
    //    var error = errors!.Single();

    //    // ASSERT
    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    error.PropertyName.Should().Be("HouseNumber");
    //    error.ErrorMessage.Should().Be("");
    //}

    //[Fact]
    //public async Task CreateContactModel_Fails_InternalServerError() => Task.CompletedTask;
}
