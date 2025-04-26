using BeaniesUtilities.Models.Enum;
using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;

namespace Resume.API.Tests.Integration.Mappings.V1;

public static class PersonModelTestMappingExtensions
{
    public static UpdatePersonModelRequest MapToUpdateRequest(this PersonModelResponse createdModel)
    {
        return new UpdatePersonModelRequest()
        {
            Id = createdModel.Id,
            Notes = "Model updated for test purposes only",
            Name = "Updated Test WorkExperience Model",

            PreferedName = createdModel.PreferedName,
            Pronouns = createdModel.Pronouns.Select(x => Enum.Parse(typeof(ePronoun), x)).Cast<ePronoun>().ToList(),
            Emails = createdModel.Emails,
            Socials = createdModel.Socials,
            Addresses = createdModel.Addresses.Select(x => x.Id).ToArray(),
            PhoneNumbers = createdModel.PhoneNumbers.Select(x => x.Id).ToArray(),
            Website = createdModel.Website,
        };
    }
}
