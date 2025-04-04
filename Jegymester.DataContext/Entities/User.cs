﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    //Felhasználó
    public class User : AbstractEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //Kapcsolatok
        public List<Booking> Bookings { get; set; }
        public int RoleId { get; set; }
        public Role Roles { get; set; }
    }
}
