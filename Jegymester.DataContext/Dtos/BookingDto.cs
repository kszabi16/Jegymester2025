using Jegymester.DataContext.Entities;
using Jegymester.DataContext.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketType = Jegymester.DataContext.Entities.TicketType;

namespace Jegymester.DataContext.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime BuyDate { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public List<TicketDto> Tickets { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateUserBookingDto
    {
        public int ScreeningId { get; set; }
        public List<int> SeatId { get; set; }   
        public TicketType TicketType { get; set; }
    }
    public class CreateGuestBookingDto
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public int ScreeningId { get; set; }
        public List<int> SeatIds { get; set; }
        public TicketType TicketType { get; set; }
    }
    public class CashierBookingDto
    {
        public int? UserId { get; set; } 
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public int ScreeningId { get; set; }
        public TicketType TicketType { get; set; }
        public List<int> SeatIds{ get; set; }
    }

}
