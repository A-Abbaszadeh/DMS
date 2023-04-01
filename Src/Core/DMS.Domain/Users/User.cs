using DMS.Domain.Attributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Users
{
    [Auditable]
    public class User:IdentityUser
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}
