using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;

namespace Resume.API.Tests.Integration.Mappings.V1;

public static class ContactModelTestMappingExtensions
{
    public static UpdateContactModelRequest MapToUpdateRequest(this ContactModelResponse createdModel)
    {
        return new UpdateContactModelRequest()
        {
            Id = createdModel.Id,
            Notes = "Model updated for test purposes only",
            Name = "Updated Test WorkExperience Model",

            Emails = createdModel.Emails,
            PhoneNumbers = createdModel.PhoneNumbers.Select(x=>x.Id).ToArray(),
            Socials = createdModel.Socials,
        };
    }
}