using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Dtos
{
    public class ScreeningDto
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
        public int AvailableSeats { get; set; }

    }
   
    public class ScreeningCreateDto
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int AvailableSeats { get; set; }
    }
    public class ScreeningUpdateDto
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int AvailableSeats { get; set; }
    }
}
