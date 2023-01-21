using System;
using Microsoft.AspNetCore.Identity;

namespace POC.SPAL.Api.Models.Users
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
