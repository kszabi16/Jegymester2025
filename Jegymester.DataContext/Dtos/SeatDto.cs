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
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string SeatNumber { get; set; }
        public bool IsOccupied { get; set; }
    }

    public class SeatCreateDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        [StringLength(10)]
        public string SeatNumber { get; set; }
    }

    public class SeatUpdateDto
    {
        [Required]
        [StringLength(10)]
        public string SeatNumber { get; set; }

        public bool IsOccupied { get; set; }
    }
}
