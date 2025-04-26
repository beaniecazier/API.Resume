using BeaniesUtilities.Models.Enum;
using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;

namespace Resume.API.Tests.Integration.Mappings.V1;

public static class AddressModelTestMappingExtensions
{
    public static UpdateAddressModelRequest MapToUpdateRequest(this AddressModelResponse createdModel)
    {
        var streetTypeSuccess = Enum.TryParse(createdModel.StreetType, out eStreetType streetType);
        if (!streetTypeSuccess)
        {
            throw new Exception($"Street type {createdModel.StreetType} is not supported for StreetType");
        }
        
        var prefixTypeSuccess = Enum.TryParse(createdModel.StreetType, out eStreetType prefixType);
        if (!prefixTypeSuccess)
        {
            throw new Exception($"Street type {createdModel.StreetType} is not supported for PrefixType");
        }
        
        var suffixTypeSuccess = Enum.TryParse(createdModel.StreetType, out eStreetType suffixType);
        if (!suffixTypeSuccess)
        {
            throw new Exception($"Street type {createdModel.StreetType} is not supported for SuffixType");
        }
        
        return new UpdateAddressModelRequest()
        {
            Id = createdModel.Id,
            Notes = "Model updated for test purposes only",
            Name = "Updated Test WorkExperience Model",

            HouseNumber = createdModel.HouseNumber,
            StreetName = createdModel.StreetName,
            StreetType = streetType,
            City = createdModel.City,
            Region = createdModel.Region,
            State = createdModel.State,
            Country = createdModel.Country,
            PostalCode = createdModel.PostalCode,
            Zip4 = createdModel.Zip4,
            CrossStreetName = createdModel.CrossStreetName,
            PrefixDirection = createdModel.PrefixDirection,
            PrefixType = prefixType,
            SuffixDirection = createdModel.SuffixDirection,
            SuffixType = suffixType,
        };
    }
}
