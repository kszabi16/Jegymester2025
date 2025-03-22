using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    //Szerepkör
    public class Role : AbstractEntity
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
