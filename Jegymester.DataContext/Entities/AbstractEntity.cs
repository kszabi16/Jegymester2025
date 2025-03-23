using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    public abstract class AbstractEntity
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
    }
}
