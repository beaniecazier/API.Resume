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

public static class OrganizationModelContractMapping
{
    public static OrganizationModel MapToModelFromCreateRequest(this CreateOrganizationModelRequest request,
        int id, string username, List<AddressModel> addresseses, List<PhoneNumberModel> phoneNumberses)
    {
        return new OrganizationModel(id, request.Name, username, request.Notes)
        {
			Industry = request.Industry,
			Status = request.Status,
			Addresses = addresseses,
			Website = request.Website,
			Departments = request.Departments,
			Emails = request.Emails,
			Socials = request.Socials,
			PhoneNumbers = phoneNumberses,
        };
    }

    public static OrganizationModel MapToModelFromUpdateRequest(this UpdateOrganizationModelRequest request, string username, List<AddressModel> addresseses, List<PhoneNumberModel> phoneNumberses)
    {
        string name = request.Name;
        return new OrganizationModel(request.Id, name, username, request.Notes)
        {
			Industry = request.Industry.Value,
			Status = request.Status.Value,
			Addresses = addresseses,
			Website = request.Website,
			Departments = request.Departments,
			Emails = request.Emails,
			Socials = request.Socials,
			PhoneNumbers = phoneNumberses,
        };
    }

    public static GetAllOrganizationModelsOptions MapToOptions(this GetAllOrganizationModelsRequest request)
    {
        return new GetAllOrganizationModelsOptions()
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

    public static GetAllOrganizationModelsOptions WithID(this GetAllOrganizationModelsOptions options, int? id)
    {
        options.ID = id;
        return options;
    }

    public static OrganizationModelResponse MapToResponseFromModel(this OrganizationModel model)
    {
        return new OrganizationModelResponse
        {
            Id = model.CommonIdentity,
            Name = model.Name,
            
			Industry = model.Industry.ToString(),
			Status = model.Status.ToString(),
			Addresses = model.Addresses.Select(m => m.MapToResponseFromModel()).ToList(),
			Website = model.Website,
			Departments = model.Departments,
			Emails = model.Emails,
			Socials = model.Socials,
			PhoneNumbers = model.PhoneNumbers.Select(m => m.MapToResponseFromModel()).ToList(),
        };
    }
}

#pragma warning restore CS1591