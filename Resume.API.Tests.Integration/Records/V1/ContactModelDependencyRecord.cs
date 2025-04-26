namespace Resume.API.Tests.Integration.Records.V1;

public record ContactModelDependencyRecord
{
	public required int[] PhoneNumbers { get; set; }
}