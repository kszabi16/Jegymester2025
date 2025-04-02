using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Dtos
{
    public class SeatDto
    {
        [Required]
        public string Row { get; set; }

        [Required]
        public string Column { get; set; }

        [Required]
        public string TicketType { get; set; }

    }
}
