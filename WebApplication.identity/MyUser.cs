using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.identity
{
    public class MyUser : IdentityUser
    {
        public string NomeCompleto { get; set; }
        public string OrgId{ get; set; }
    }

    public class Organization
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
