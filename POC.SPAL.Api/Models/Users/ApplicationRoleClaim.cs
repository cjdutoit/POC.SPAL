using System;
using Microsoft.AspNetCore.Identity;

namespace POC.SPAL.Api.Models.Users
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
