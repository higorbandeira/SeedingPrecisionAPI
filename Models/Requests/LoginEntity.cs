using System;
using System.ComponentModel.DataAnnotations;

namespace SeedingPrecision.Models.Requests
{
    public class LoginEntity
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
