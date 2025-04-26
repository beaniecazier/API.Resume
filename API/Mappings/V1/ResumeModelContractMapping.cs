using BeaniesUtilities.Models.CommonModels;
using BeaniesUtilities.Models.Resume;
using Gay.TCazier.Resume.BLL.Options.V1;
using Gay.TCazier.Resume.Contracts.Requests.V1.Create;
using Gay.TCazier.Resume.Contracts.Requests.V1.GetAll;
using Gay.TCazier.Resume.Contracts.Requests.V1.Update;
using Gay.TCazier.Resume.Contracts.Responses.V1;
using Microsoft.Data.SqlClient;

namespace Gay.TCazier.Resume.API.Mappings.V1;

#pragma warning disable CS1591

public static class ResumeModelContractMapping
{
    public static ResumeModel MapToModelFromCreateRequest(this CreateResumeModelRequest request,
        int id, string username, List<EducationDegreeModel> degreeses, List<CertificateModel> certificateses, List<WorkExperienceModel> workExperiences, List<ProjectModel> projectses, List<AddressModel> addresseses, List<PhoneNumberModel> phoneNumberses)
    {
        return new ResumeModel(id, request.Name, username, request.Notes)
        {
			HeroStatement = request.HeroStatement,
			Degrees = degreeses,
			Certificates = certificateses,
			WorkExperience = workExperiences,
			Projects = projectses,
			PreferedName = request.PreferedName,
			Pronouns = request.Pronouns,
			Addresses = addresseses,
			Website = request.Website,
			Emails = request.Emails,
			Socials = request.Socials,
			PhoneNumbers = phoneNumberses,
        };
    }

    public static ResumeModel MapToModelFromUpdateRequest(this UpdateResumeModelRequest request, string username, List<EducationDegreeModel> degreeses, List<CertificateModel> certificateses, List<WorkExperienceModel> workExperiences, List<ProjectModel> projectses, List<AddressModel> addresseses, List<PhoneNumberModel> phoneNumberses)
    {
        string name = request.Name;
        return new ResumeModel(request.Id, name, username, request.Notes)
        {
			HeroStatement = request.HeroStatement,
			Degrees = degreeses,
			Certificates = certificateses,
			WorkExperience = workExperiences,
			Projects = projectses,
			PreferedName = request.PreferedName,
			Pronouns = request.Pronouns,
			Addresses = addresseses,
			Website = request.Website,
			Emails = request.Emails,
			Socials = request.Socials,
			PhoneNumbers = phoneNumberses,
        };
    }

    public static GetAllResumeModelsOptions MapToOptions(this GetAllResumeModelsRequest request)
    {
        return new GetAllResumeModelsOptions()
        {
            NameSearchTerm = request.NameSearchTerm,
            NotesSearchTerm = request.NotesSearchTerm,
            AfterDate = request.AfterDate,
            BeforeDate = request.BeforeDate,
            AllowHidden = request.AllowHidden,
            AllowDeleted = request.AllowDeleted,
            GreaterThanOrEqualToID = request.GreaterThanOrEqualToID,
            LessThanOrEqualToID = request.LessThanOrEqualToID,
            SpecificIds = request.SpecificIds,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unspecified : 
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
        };
    }

    public static GetAllResumeModelsOptions WithID(this GetAllResumeModelsOptions options, int? id)
    {
        options.ID = id;
        return options;
    }

    public static ResumeModelResponse MapToResponseFromModel(this ResumeModel model)
    {
        return new ResumeModelResponse
        {
            Id = model.CommonIdentity,
            Name = model.Name,
            
			HeroStatement = model.HeroStatement,
			Degrees = model.Degrees.Select(m => m.MapToResponseFromModel()).ToList(),
			Certificates = model.Certificates.Select(m => m.MapToResponseFromModel()).ToList(),
			WorkExperience = model.WorkExperience.Select(m => m.MapToResponseFromModel()).ToList(),
			Projects = model.Projects.Select(m => m.MapToResponseFromModel()).ToList(),
			PreferedName = model.PreferedName,
			Pronouns = model.Pronouns.Select(x=>x.ToString()).ToList(),
			Addresses = model.Addresses.Select(m => m.MapToResponseFromModel()).ToList(),
			Website = model.Website,
			Emails = model.Emails,
			Socials = model.Socials,
			PhoneNumbers = model.PhoneNumbers.Select(m => m.MapToResponseFromModel()).ToList(),
        };
    }
}

#pragma warning restore CS1591