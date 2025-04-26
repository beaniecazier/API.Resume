namespace Resume.API.Tests.Integration.Records.V1;

public record WorkExperienceModelDependencyRecord
{
	public required int Company { get; set; }
	public required int[] TechUsed { get; set; }
}