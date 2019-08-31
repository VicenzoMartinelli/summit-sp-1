using Microsoft.AspNetCore.Identity;

namespace SummitApi.Domain
{
    public class Usuario : IdentityUser
    {
        public string QualquerCoisa { get; set; }
    }
}