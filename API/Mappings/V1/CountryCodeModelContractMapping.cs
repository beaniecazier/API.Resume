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

public static class CountryCodeModelContractMapping
{
    public static CountryCodeModel MapToModelFromCreateRequest(this CreateCountryCodeModelRequest request,
        int id, string username)
    {
        return new CountryCodeModel(id, request.Name, username, request.Notes)
        {
			Country = request.Country,
			CallingCode = request.CallingCode,
			Iso3Letter = request.Iso3Letter,
			Iso2Letter = request.Iso2Letter,
			IsoNumeric = request.IsoNumeric,
        };
    }

    public static CountryCodeModel MapToModelFromUpdateRequest(this UpdateCountryCodeModelRequest request, string username)
    {
        string name = request.Name;
        return new CountryCodeModel(request.Id, name, username, request.Notes)
        {
			Country = request.Country,
			CallingCode = request.CallingCode,
			Iso3Letter = request.Iso3Letter,
			Iso2Letter = request.Iso2Letter,
			IsoNumeric = request.IsoNumeric.Value,
        };
    }

    public static GetAllCountryCodeModelsOptions MapToOptions(this GetAllCountryCodeModelsRequest request)
    {
        return new GetAllCountryCodeModelsOptions()
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

    public static GetAllCountryCodeModelsOptions WithID(this GetAllCountryCodeModelsOptions options, int? id)
    {
        options.ID = id;
        return options;
    }

    public static CountryCodeModelResponse MapToResponseFromModel(this CountryCodeModel model)
    {
        return new CountryCodeModelResponse
        {
            Id = model.CommonIdentity,
            Name = model.Name,
            
			Country = model.Country,
			CallingCode = model.CallingCode,
			Iso3Letter = model.Iso3Letter,
			Iso2Letter = model.Iso2Letter,
			IsoNumeric = model.IsoNumeric,
        };
    }
}

#pragma warning restore CS1591