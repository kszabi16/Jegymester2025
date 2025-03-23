using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    //Filmek
    public class Movie : AbstractEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Length { get; set; }
        public string Director { get; set; }
        public string MovieType { get; set; }
        public int AgeLimit { get; set; }

        //Kapcsolatok
        public List<Screening> Screenings { get; set; }
    }
}
