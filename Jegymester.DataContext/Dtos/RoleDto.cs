using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class UpdateUserRolesDto
    {
        [Required]
        public int UserId { get; set; }

        public List<string> Roles { get; set; } 
    }
}
