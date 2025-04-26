using BeaniesUtilities.Models.Enum;
using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;

namespace Resume.API.Tests.Integration.Mappings.V1;

public static class ResumeModelTestMappingExtensions
{
    public static UpdateResumeModelRequest MapToUpdateRequest(this ResumeModelResponse createdModel)
    {
        List<ePronoun> pronouns = createdModel.Pronouns.Select(x =>
        {
            var success = Enum.TryParse(x, out ePronoun pronoun);
            if (!success)
            {
                throw new Exception($"Invalid category: {x}");
            }
            return pronoun;
        }).ToList();

        return new UpdateResumeModelRequest()
        {
            Id = createdModel.Id,
            Notes = "Model updated for test purposes only",
            Name = "Updated Test WorkExperience Model",

            HeroStatement = createdModel.HeroStatement,
            Degrees = createdModel.Degrees.Select(x => x.Id).ToArray(),
            Certificates = createdModel.Certificates.Select(x => x.Id).ToArray(),
            WorkExperience = createdModel.WorkExperience.Select(x => x.Id).ToArray(),
            Projects = createdModel.Projects.Select(x => x.Id).ToArray(),
            PreferedName = createdModel.PreferedName,
            Pronouns = pronouns,
            Emails = createdModel.Emails,
            Socials = createdModel.Socials,
            Addresses = createdModel.Addresses.Select(x => x.Id).ToArray(),
            PhoneNumbers = createdModel.PhoneNumbers.Select(x => x.Id).ToArray(),
            Website = createdModel.Website,
        };
    }
}
