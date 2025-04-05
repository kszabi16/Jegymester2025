using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Dtos
{
    public class MovieDto 
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Length { get; set; }
        public string Director { get; set; }
        public string MovieType { get; set; }
        public int AgeLimit { get; set; }

    }

    public class MovieCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Length { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string MovieType { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string AgeLimit { get; set; }
    }

    public class MovieUpdateDto 
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Length { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string MovieType { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string AgeLimit { get; set; }

    }
}
