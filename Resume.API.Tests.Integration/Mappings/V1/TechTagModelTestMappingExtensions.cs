using BeaniesUtilities.Models.Enum;
using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;
using LanguageExt;

namespace Resume.API.Tests.Integration.Mappings.V1;

public static class TechTagModelTestMappingExtensions
{
    public static UpdateTechTagModelRequest MapToUpdateRequest(this TechTagModelResponse createdModel)
    {
        List<eSkillCategory> categories = createdModel.Category.Select(x =>
        {
            var success = Enum.TryParse(x, out eSkillCategory category);
            if (!success)
            {
                throw new Exception($"Invalid category: {x}");
            }
            return category;
        }).ToList();

        return new UpdateTechTagModelRequest()
        {
            Id = createdModel.Id,
            Notes = "Model updated for test purposes only",
            Name = "Updated Test WorkExperience Model",

            Category = categories,
            URL = createdModel.URL,
            Description = createdModel.Description,
        };
    }
}
