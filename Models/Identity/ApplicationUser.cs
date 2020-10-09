using System;
using AspNetCore.Identity.Mongo;

namespace SeedingPrecision.Models.Identity
{
    //Add any custom field for a user
    public class ApplicationUser : MongoIdentityUser
    {
        public string Name { get; set; }

        public string Farm { get; set; }
    }
}
