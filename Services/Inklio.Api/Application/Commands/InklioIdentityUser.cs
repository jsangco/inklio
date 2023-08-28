using Microsoft.AspNetCore.Identity;

public class InklioIdentityUser : IdentityUser<Guid>
{
    public InklioIdentityUser() : base()
    {
    }

    public InklioIdentityUser(string name) : base(name)
    {
    }
}