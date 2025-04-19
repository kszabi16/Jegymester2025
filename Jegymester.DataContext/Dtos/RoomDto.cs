using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
    }

    public class RoomCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
    }

    public class RoomUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
    }
}
