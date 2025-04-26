using BeaniesUtilities.Models.Enum;
using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;

namespace Resume.API.Tests.Integration.Mappings.V1;

public static class OrganizationModelTestMappingExtensions
{
    public static UpdateOrganizationModelRequest MapToUpdateRequest(this OrganizationModelResponse createdModel)
    {
        var industrySuccess = Enum.TryParse(createdModel.Industry, out eIndustry industry);
        if (!industrySuccess)
        {
            throw new Exception($"Employment type {createdModel.Industry} is not valid");
        }

        var statusSuccess = Enum.TryParse(createdModel.Status, out eOrganizationStatus status);
        if (!statusSuccess)
        {
            throw new Exception($"Employment type {createdModel.Status} is not valid");
        }

        return new UpdateOrganizationModelRequest()
        {
            Id = createdModel.Id,
            Notes = "Model updated for test purposes only",
            Name = "Updated Test WorkExperience Model",

            Addresses = createdModel.Addresses.Select(x=>x.Id).ToArray(),
            Website = createdModel.Website,
            Departments = createdModel.Departments,
            Industry = industry,
            Socials = createdModel.Socials,
            Status = status,
            Emails = createdModel.Emails,
            PhoneNumbers = createdModel.PhoneNumbers.Select(x=>x.Id).ToArray(),
        };
    }
}