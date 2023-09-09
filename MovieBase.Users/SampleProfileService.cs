
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace MovieBase.Users;

public class SampleProfileService : DefaultProfileService
{
    public SampleProfileService(ILogger<DefaultProfileService> logger) : base(logger)
    {
    }

    public override Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        context.IssuedClaims.AddRange(context.Subject.Claims.Where(c=>"nameemailrole".Contains(c.Type)));
        return Task.CompletedTask;
    }
}
