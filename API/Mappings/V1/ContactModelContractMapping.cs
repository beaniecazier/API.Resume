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

public static class ContactModelContractMapping
{
    public static ContactModel MapToModelFromCreateRequest(this CreateContactModelRequest request,
        int id, string username, List<PhoneNumberModel> phoneNumberses)
    {
        return new ContactModel(id, request.Name, username, request.Notes)
        {
			Emails = request.Emails,
			Socials = request.Socials,
			PhoneNumbers = phoneNumberses,
        };
    }

    public static ContactModel MapToModelFromUpdateRequest(this UpdateContactModelRequest request, string username, List<PhoneNumberModel> phoneNumberses)
    {
        string name = request.Name;
        return new ContactModel(request.Id, name, username, request.Notes)
        {
			Emails = request.Emails,
			Socials = request.Socials,
			PhoneNumbers = phoneNumberses,
        };
    }

    public static GetAllContactModelsOptions MapToOptions(this GetAllContactModelsRequest request)
    {
        return new GetAllContactModelsOptions()
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

    public static GetAllContactModelsOptions WithID(this GetAllContactModelsOptions options, int? id)
    {
        options.ID = id;
        return options;
    }

    public static ContactModelResponse MapToResponseFromModel(this ContactModel model)
    {
        return new ContactModelResponse
        {
            Id = model.CommonIdentity,
            Name = model.Name,
            
			Emails = model.Emails,
			Socials = model.Socials,
			PhoneNumbers = model.PhoneNumbers.Select(m => m.MapToResponseFromModel()).ToList(),
        };
    }
}

#pragma warning restore CS1591