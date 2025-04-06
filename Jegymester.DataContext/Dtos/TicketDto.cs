using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public int ScreeningId { get; set; }
        public DateTime ScreeningTime { get; set; }
        public string Title { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public class CashierTicketPurchaseDto
    {
        [Required]
        public int ScreeningId { get; set; }

        [Required]
        [Range(1, 10)]
        public int TicketCount { get; set; }

        public int? UserId { get; set; } // null if guest purchase

        [EmailAddress]
        public string? CustomerEmail { get; set; }

        [Phone]
        public string? CustomerPhone { get; set; }

    }
    public class ValidateTicketDto
    {
        [Required]
        public int TicketId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
