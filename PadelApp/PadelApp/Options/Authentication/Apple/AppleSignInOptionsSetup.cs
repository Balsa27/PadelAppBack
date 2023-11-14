using Microsoft.Extensions.Options;

namespace PadelApp.Options.Authentication.Apple;

public class AppleSignInOptionsSetup : IConfigureOptions<AppleSignInOptions>
{
    private const string SectionName = "Apple";
    private readonly IConfiguration _configuration;

    
    public void Configure(AppleSignInOptions options)
    {
        _configuration.GetSection("Apple").Bind(options);
    }
}