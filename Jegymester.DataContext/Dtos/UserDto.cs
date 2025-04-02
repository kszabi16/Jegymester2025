using Jegymester.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Dtos
{
  public  class UserDto 
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phonenumer { get; set; }
        public IList<RoleDto> Roles { get; set; }
    }

    public class RegisterUserDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public IList<int> RoleIds { get; set; }
    }
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public IList<int> RoleIds { get; set; }
    }
}
