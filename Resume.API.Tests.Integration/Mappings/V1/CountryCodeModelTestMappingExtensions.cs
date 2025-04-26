using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;

namespace Resume.API.Tests.Integration.Mappings.V1;

public static class CountryCodeModelTestMappingExtensions
{
    public static UpdateCountryCodeModelRequest MapToUpdateRequest(this CountryCodeModelResponse createdModel)
    {
        return new UpdateCountryCodeModelRequest()
        {
            Id = createdModel.Id,
            Notes = "Model updated for test purposes only",
            Name = "Updated Test WorkExperience Model",

            Country = createdModel.Country,
            CallingCode = createdModel.CallingCode,
            Iso3Letter = createdModel.Iso3Letter,
            Iso2Letter = createdModel.Iso2Letter,
            IsoNumeric = createdModel.IsoNumeric,
        };
    }
}