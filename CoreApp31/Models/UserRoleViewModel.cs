using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp31.Models
{
    public class UserRoleViewModel
    {
        public  string UserId { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
