using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    //Felhasználó
    public class User : AbstractEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PasswordHash { get; set; }


        //Kapcsolatok
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<RoleType> Roles { get; set; } 
    }
}
