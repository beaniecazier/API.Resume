using Gay.TCazier.Resume.Contracts.Endpoints.V1;
using Gay.TCazier.Resume.Contracts.Requests.V1.GetAll;

namespace Gay.TCazier.Resume.API.OutputCache;

#pragma warning disable CS1591

public static class OutputCacheServiceExtensions
{
    public static int OutputCacheExpirationInMinutes = 1;

    public static IServiceCollection AddOutputAndResponseCacheing(this IServiceCollection services)
    {
        //services.AddResponseCaching();
        services.AddOutputCache(x =>
        {
            x.AddBasePolicy(c => c.Cache());
            
            x.AddPolicy(AddressModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllAddressModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(AddressModelEndpoints.Tag);
            });

            x.AddPolicy(ContactModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllContactModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(ContactModelEndpoints.Tag);
            });

            x.AddPolicy(CountryCodeModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllCountryCodeModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(CountryCodeModelEndpoints.Tag);
            });

            x.AddPolicy(OrganizationModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllOrganizationModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(OrganizationModelEndpoints.Tag);
            });

            x.AddPolicy(PersonModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllPersonModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(PersonModelEndpoints.Tag);
            });

            x.AddPolicy(PhoneNumberModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllPhoneNumberModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(PhoneNumberModelEndpoints.Tag);
            });

            x.AddPolicy(CertificateModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllCertificateModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(CertificateModelEndpoints.Tag);
            });

            x.AddPolicy(EducationDegreeModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllEducationDegreeModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(EducationDegreeModelEndpoints.Tag);
            });

            x.AddPolicy(ProjectModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllProjectModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(ProjectModelEndpoints.Tag);
            });

            x.AddPolicy(ResumeModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllResumeModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(ResumeModelEndpoints.Tag);
            });

            x.AddPolicy(TechTagModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllTechTagModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(TechTagModelEndpoints.Tag);
            });

            x.AddPolicy(WorkExperienceModelEndpoints.Tag, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(OutputCacheExpirationInMinutes))
                    .SetVaryByQuery(typeof(GetAllWorkExperienceModelsRequest).GetProperties().Select(p => p.Name).ToArray())
                    .Tag(WorkExperienceModelEndpoints.Tag);
            });
        });
        return services;
    }
}

#pragma warning restore CS1591