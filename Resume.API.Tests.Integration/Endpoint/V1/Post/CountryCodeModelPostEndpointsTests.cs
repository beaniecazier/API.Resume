using BeaniesUtilities.Models.Resume;
using FluentAssertions;
using FluentValidation.Results;
using Gay.TCazier.Resume.API.Endpoints.V1.Create;
using Gay.TCazier.Resume.API.Endpoints.V1.Get;
using Gay.TCazier.Resume.API.Endpoints.V1.Delete;
using Gay.TCazier.Resume.Contracts.Requests.V1.Create;
using Gay.TCazier.Resume.Contracts.Responses.V1;
using Gay.TCazier.Resume.Contracts.Endpoints.V1;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Resume.API.Tests.Integration.Endpoint.V1.Post;

public class CountryCodeModelPostEndpointsTests : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    private List<int> _createdCountryCodeModels = new List<int>();

    public CountryCodeModelPostEndpointsTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _factory.CreateClient();
        foreach (int id in _createdCountryCodeModels)
        {
            await httpClient.DeleteAsync($"{CountryCodeModelEndpoints.EndpointPrefix}/{id}");
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateCountryCodeModel_CreatesCountryCodeModel_WhenDataIsCorrect()
    {
        // ARRANGE
        var httpClient = _factory.CreateClient();
        
        var modelRecord = await ModelGenerator.PopulateDatabaseForCountryCodeModelTest(httpClient);
        var modelRequest = ModelGenerator.GenerateNewCreateCountryCodeModelRequest(modelRecord);

        // ACT
        var result = await httpClient.PostAsJsonAsync(CountryCodeModelEndpoints.Post, modelRequest);
        var url = result.Headers.Location.AbsolutePath;
        var get = await httpClient.GetAsync(url);
        var createdModel = await get.Content.ReadFromJsonAsync<CountryCodeModelResponse>();
        _createdCountryCodeModels.Add(createdModel.Id);

        // ASSERT
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        createdModel.Should().BeEquivalentTo(createdModel);
        result.Headers.Location.AbsolutePath.Should().Be($"/{CountryCodeModelEndpoints.EndpointPrefix}/{createdModel.Id}");
    }

    //[Fact]
    //public async Task CreateCountryCodeModel_Fails_WhenHouseNumberIsInvalid()
    //{
    //    // ARRANGE
    //    var httpClient = _factory.CreateClient();
    //    var modelRequest = ModelGenerator.GenerateNewCreateCountryCodeModelRequest();

    //    modelRequest.HouseNumber = null;

    //    // ACT
    //    var result = await httpClient.PostAsJsonAsync(CountryCodeModelEndpoints.Post, modelRequest);
    //    var errors = await result.Content.ReadFromJsonAsync<IEnumerable<ValidationFailure>>();
    //    var error = errors!.Single();

    //    // ASSERT
    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    error.PropertyName.Should().Be("HouseNumber");
    //    error.ErrorMessage.Should().Be("");
    //}

    //[Fact]
    //public async Task CreateCountryCodeModel_Fails_InternalServerError() => Task.CompletedTask;

    [Fact]
    public async Task CreateCountryCodeModel_PopulateFromList_WhenDataIsCorrect()
    {
        List<string> countryCodes = new()
        {
            "\"Afghanistan\",93,AF,AFG,004",
            "\"Albania\",355,AL,ALB,008",
            "\"Algeria\",213,DZ,DZA,012",
            "\"American Samoa\",1-684,AS,ASM,016",
            "\"Andorra\",376,AD,AND,020",
            "\"Angola\",244,AO,AGO,024",
            "\"Anguilla\",1-264,AI,AIA,660",
            "\"Antartica\",672,AQ,ATA,010",
            "\"Antigua and Barbuda\",1-268,AG,ATG,028",
            "\"Argentina\",54,AR,ARG,032",
            "\"Armenia\",374,AM,ARM,051",
            "\"Aruba\",297,AW,ABW,533",
            "\"Australia\",61,AU,AUS,036",
            "\"Austria\",43,AT,AUT,040",
            "\"Azerbaijan\",994,AZ,AZE,031",
            "\"Bahamas\",1-242,BS,BHS,044",
            "\"Bahrain\",973,BH,BHR,048",
            "\"Bangladesh\",880,BD,BGD,050",
            "\"Barbados\",1-246,BB,BRB,052",
            "\"Belarus\",375,BY,BLR,112",
            "\"Belgium\",32,BE,BEL,056",
            "\"Belize\",501,BZ,BLZ,084",
            "\"Benin\",229,BJ,BEN,204",
            "\"Bermuda\",1-441,BM,BMU,060",
            "\"Bhutan\",975,BT,BTN,064",
            "\"Bolivia\",591,BO,BOL,068",
            "\"Bosnia and Herzegovina\",387,BA,BIH,070",
            "\"Botswana\",267,BW,BWA,072",
            "\"Brazil\",55,BR,BRA,076",
            "\"British Indian Ocean Territory\",246,IO,IOT,086",
            "\"British Virgin Islands\",1-284,VG,VGB,092",
            "\"Brunei\",673,BN,BRN,096",
            "\"Bulgaria\",359,BG,BGR,100",
            "\"Burkina Faso\",226,BF,BFA,854",
            "\"Burundi\",257,BI,BDI,108",
            "\"Cabo Verde\",238,CV,CPV,132",
            "\"Cambodia\",855,KH,KHM,116",
            "\"Cameroon\",237,CM,CMR,120",
            "\"Canada\",1,CA,CAN,124",
            "\"Cayman Islands\",1-345,KY,CYM,136",
            "\"Central African Republic\",236,CF,CAF,140",
            "\"Chad\",235,TD,TCD,148",
            "\"Chile\",56,CL,CHL,152",
            "\"China\",86,CN,CHN,156",
            "\"Christmas Island\",61,CX,CXR,162",
            "\"Cocos Islands\",61,CC,CCK,166",
            "\"Colombia\",57,CO,COL,170",
            "\"Comoros\",269,KM,COM,174",
            "\"Congo\",242,CG,COG,178",
            "\"Cook Islands\",682,CK,COK,184",
            "\"Costa Rica\",506,CR,CRI,188",
            "\"Croatia\",385,HR,HRV,191",
            "\"Cuba\",53,CU,CUB,192",
            "\"Curaçao\",599,CW,CUW,531",
            "\"Cyprus\",357,CY,CYP,196",
            "\"Czech Republic (Czechia)\",420,CZ,CZE,203",
            "\"Côte d'Ivoire\",225,CI,CIV,384",
            "\"Denmark\",45,DK,DNK,208",
            "\"Djibouti\",253,DJ,DJI,262",
            "\"Dominica\",1-767,DM,DMA,212",
            "\"Dominican Republic\",1-809,DO,DOM,214",
            "\"Dominican Republic\",1-829,DO,DOM,214",
            "\"Dominican Republic\",1-849,DO,DOM,214",
            "\"DR Congo\",243,CD,COD,180",
            "\"Ecuador\",593,EC,ECU,218",
            "\"Egypt\",20,EG,EGY,818",
            "\"El Salvador\",503,SV,SLV,222",
            "\"Equatorial Guinea\",240,GQ,GNQ,226",
            "\"Eritrea\",291,ER,ERI,232",
            "\"Estonia\",372,EE,EST,233",
            "\"Eswatini\",268,SZ,SWZ,748",
            "\"Ethiopia\",251,ET,ETH,231",
            "\"Faeroe Islands\",298,FO,FRO,234",
            "\"Falkland Islands\",500,FK,FLK,238",
            "\"Fiji\",679,FJ,FJI,242",
            "\"Finland\",358,FI,FIN,246",
            "\"France\",33,FR,FRA,250",
            "\"French Polynesia\",689,PF,PYF,258",
            "\"Gabon\",241,GA,GAB,266",
            "\"Gambia\",220,GM,GMB,270",
            "\"Georgia\",995,GE,GEO,268",
            "\"Germany\",49,DE,DEU,276",
            "\"Ghana\",233,GH,GHA,288",
            "\"Gibraltar\",350,GI,GIB,292",
            "\"Greece\",30,GR,GRC,300",
            "\"Greenland\",299,GL,GRL,304",
            "\"Grenada\",1-473,GD,GRD,308",
            "\"Guam\",1-671,GU,GUM,316",
            "\"Guatemala\",502,GT,GTM,320",
            "\"Guernsey\",44-148,GG,GGY,831",
            "\"Guinea\",224,GN,GIN,324",
            "\"Guinea-Bissau\",245,GW,GNB,624",
            "\"Guyana\",592,GY,GUY,328",
            "\"Haiti\",509,HT,HTI,332",
            "\"Holy See\",379,VA,VAT,336",
            "\"Honduras\",504,HN,HND,340",
            "\"Hong Kong\",852,HK,HKG,344",
            "\"Hungary\",36,HU,HUN,348",
            "\"Iceland\",354,IS,ISL,352",
            "\"India\",91,IN,IND,356",
            "\"Indonesia\",62,ID,IDN,360",
            "\"Iran\",98,IR,IRN,364",
            "\"Iraq\",964,IQ,IRQ,368",
            "\"Ireland\",353,IE,IRL,372",
            "\"Isle of Man\",44-162,IM,IMN,833",
            "\"Israel\",972,IL,ISR,376",
            "\"Italy\",39,IT,ITA,380",
            "\"Jamaica\",1-876,JM,JAM,388",
            "\"Japan\",81,JP,JPN,392",
            "\"Jersey\",44-153,JE,JEY,832",
            "\"Jordan\",962,JO,JOR,400",
            "\"Kazakhstan\",7,KZ,KAZ,398",
            "\"Kenya\",254,KE,KEN,404",
            "\"Kiribati\",686,KI,KIR,296",
            "\"Kuwait\",965,KW,KWT,414",
            "\"Kyrgyzstan\",996,KG,KGZ,417",
            "\"Laos\",856,LA,LAO,418",
            "\"Latvia\",371,LV,LVA,428",
            "\"Lebanon\",961,LB,LBN,422",
            "\"Lesotho\",266,LS,LSO,426",
            "\"Liberia\",231,LR,LBR,430",
            "\"Libya\",218,LY,LBY,434",
            "\"Liechtenstein\",423,LI,LIE,438",
            "\"Lithuania\",370,LT,LTU,440",
            "\"Luxembourg\",352,LU,LUX,442",
            "\"Macao\",853,MO,MAC,446",
            "\"Madagascar\",261,MG,MDG,450",
            "\"Malawi\",265,MW,MWI,454",
            "\"Malaysia\",60,MY,MYS,458",
            "\"Maldives\",960,MV,MDV,462",
            "\"Mali\",223,ML,MLI,466",
            "\"Malta\",356,MT,MLT,470",
            "\"Marshall Islands\",692,MH,MHL,584",
            "\"Mauritania\",222,MR,MRT,478",
            "\"Mauritius\",230,MU,MUS,480",
            "\"Mayotte\",262,YT,MYT,175",
            "\"Mexico\",52,MX,MEX,484",
            "\"Micronesia\",691,FM,FSM,583",
            "\"Moldova\",373,MD,MDA,498",
            "\"Monaco\",377,MC,MCO,492",
            "\"Mongolia\",976,MN,MNG,496",
            "\"Montenegro\",382,ME,MNE,499",
            "\"Montserrat\",1-664,MS,MSR,500",
            "\"Morocco\",212,MA,MAR,504",
            "\"Mozambique\",258,MZ,MOZ,508",
            "\"Myanmar\",95,MM,MMR,104",
            "\"Namibia\",264,NA,NAM,516",
            "\"Nauru\",674,NR,NRU,520",
            "\"Nepal\",977,NP,NPL,524",
            "\"Netherlands\",31,NL,NLD,528",
            "\"New Caledonia\",687,NC,NCL,540",
            "\"New Zealand\",64,NZ,NZL,554",
            "\"Nicaragua\",505,NI,NIC,558",
            "\"Niger\",227,NE,NER,562",
            "\"Nigeria\",234,NG,NGA,566",
            "\"Niue\",683,NU,NIU,570",
            "\"North Kore\",850,KP,PRK,408",
            "\"North Macedonia\",389,MK,MKD,807",
            "\"Northern Mariana Islands\",1-670,MP,MNP,580",
            "\"Norway\",47,NO,NOR,578",
            "\"Oman\",968,OM,OMN,512",
            "\"Pakistan\",92,PK,PAK,586",
            "\"Palau\",680,PW,PLW,585",
            "\"Panama\",507,PA,PAN,591",
            "\"Papua New Guinea\",675,PG,PNG,598",
            "\"Paraguay\",595,PY,PRY,600",
            "\"Peru\",51,PE,PER,604",
            "\"Philippines\",63,PH,PHL,608",
            "\"Pitcairn Islands\",64,PN,PCN,612",
            "\"Poland\",48,PL,POL,616",
            "\"Portugal\",351,PT,PRT,620",
            "\"Puerto Rico\",1-787,PR,PRI,630",
            "\"Puerto Rico\",1-939,PR,PRI,630",
            "\"Qatar\",974,QA,QAT,634",
            "\"Romania\",40,RO,ROU,642",
            "\"Russia\",7,RU,RUS,643",
            "\"Rwanda\",250,RW,RWA,646",
            "\"Réunion\",262,RE,REU,638",
            "\"Saint Barthelemy\",590,BL,BLM,652",
            "\"Saint Helena\",290,SH,SHN,654",
            "\"Saint Kitts & Nevis\",1-869,KN,KNA,659",
            "\"Saint Lucia\",1-758,LC,LCA,662",
            "\"Saint Martin\",590,MF,MAF,663",
            "\"Saint Pierre & Miquelon\",508,PM,SPM,666",
            "\"Samoa\",685,WS,WSM,882",
            "\"San Marino\",378,SM,SMR,674",
            "\"Sao Tome & Principe\",239,ST,STP,678",
            "\"Saudi Arabia\",966,SA,SAU,682",
            "\"Senegal\",221,SN,SEN,686",
            "\"Serbia\",381,RS,SRB,688",
            "\"Seychelles\",248,SC,SYC,690",
            "\"Sierra Leone\",232,SL,SLE,694",
            "\"Singapore\",65,SG,SGP,702",
            "\"Sint Maarten\",1-721,SX,SXM,534",
            "\"Slovakia\",421,SK,SVK,703",
            "\"Slovenia\",386,SI,SVN,705",
            "\"Solomon Islands\",677,SB,SLB,090",
            "\"Somalia\",252,SO,SOM,706",
            "\"South Africa\",27,ZA,ZAF,710",
            "\"South Korea\",82,KR,KOR,410",
            "\"South Sudan\",211,SS,SSD,728",
            "\"Spain\",34,ES,ESP,724",
            "\"Sri Lanka\",94,LK,LKA,144",
            "\"St. Vincent & Grenadines\",1-784,VC,VCT,670",
            "\"State of Palestine\",970,PS,PSE,275",
            "\"Sudan\",249,SD,SDN,729",
            "\"Suriname\",597,SR,SUR,740",
            "\"Svalbard and Jan Mayen\",47,SJ,SJM,744",
            "\"Sweden\",46,SE,SWE,752",
            "\"Switzerland\",41,CH,CHE,756",
            "\"Syria\",963,SY,SYR,760",
            "\"Taiwan\",886,TW,TWN,158",
            "\"Tajikistan\",992,TJ,TJK,762",
            "\"Tanzania\",255,TZ,TZA,834",
            "\"Thailand\",66,TH,THA,764",
            "\"Timor-Leste\",670,TL,TLS,626",
            "\"Togo\",228,TG,TGO,768",
            "\"Tokelau\",690,TK,TKL,772",
            "\"Tonga\",676,TO,TON,776",
            "\"Trinidad and Tobago\",1-868,TT,TTO,780",
            "\"Tunisia\",216,TN,TUN,788",
            "\"Turkey\",90,TR,TUR,792",
            "\"Turkmenistan\",993,TM,TKM,795",
            "\"Turks and Caicos\",1-649,TC,TCA,796",
            "\"Tuvalu\",688,TV,TUV,798",
            "\"U.S. Virgin Islands\",1-340,VI,VIR,850",
            "\"Uganda\",256,UG,UGA,800",
            "\"Ukraine\",380,UA,UKR,804",
            "\"United Arab Emirates\",971,AE,ARE,784",
            "\"United Kingdom\",44,GB,GBR,826",
            "\"United States\",1,US,USA,840",
            "\"Uruguay\",598,UY,URY,858",
            "\"Uzbekistan\",998,UZ,UZB,860",
            "\"Vanuatu\",678,VU,VUT,548",
            "\"Venezuela\",58,VE,VEN,862",
            "\"Vietnam\",84,VN,VNM,704",
            "\"Wallis & Futuna\",681,WF,WLF,876",
            "\"Western Sahara\",212,EH,ESH,732",
            "\"Yemen\",967,YE,YEM,887",
            "\"Zambia\",260,ZM,ZMB,894",
            "\"Zimbabwe\",263,ZW,ZWE,716",
        };
        var httpClient = _factory.CreateClient();

        for (int i = 0; i < countryCodes.Count; i++)
        {
            // ARRANGE
            var fields = countryCodes[i].Split(',');
            var request = new CreateCountryCodeModelRequest
            {
                Name = fields[0].Trim('\"'),
                Notes = "Model created for test purposes only",

                Country = fields[0].Trim('\"'),
                CallingCode = fields[1],
                Iso2Letter = fields[2],
                Iso3Letter = fields[3],
                IsoNumeric = uint.Parse(fields[4]),
            };
        
            // ACT
            var result = await httpClient.PostAsJsonAsync(CountryCodeModelEndpoints.Post, request);
            if (result.Headers.Location?.AbsolutePath is null)
            {
                Console.WriteLine("Country Code Model failed");
            }
            var url = result.Headers.Location.AbsolutePath;
            var get = await httpClient.GetAsync(url);
            var createdModel = await get.Content.ReadFromJsonAsync<CountryCodeModelResponse>();
            _createdCountryCodeModels.Add(createdModel.Id);

            // ASSERT
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            createdModel.Should().BeEquivalentTo(createdModel);
            result.Headers.Location.AbsolutePath.Should().Be($"/{CountryCodeModelEndpoints.EndpointPrefix}/{createdModel.Id}");
        }
        Console.WriteLine("Country Code Model failed");
    }
}
