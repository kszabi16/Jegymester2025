using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.Services
{
    public interface ICashierService
    {
        Task<TicketDto> PurchaseTicketForUserAsync(int screeningId, int seatId, string userId);
        Task<TicketDto> PurchaseTicketForGuestAsync(int screeningId, int seatId, GuestTicketPurchaseDto guestDto);
        Task<bool> ValidateTicketAsync(int ticketId);
    }

    public class CashierService : ICashierService
    {
        private readonly IMapper _mapper;
        private readonly JegymesterDbContext _context;

        public CashierService(IMapper mapper, JegymesterDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task<TicketDto> PurchaseTicketForGuestAsync(int screeningId, int seatId, GuestTicketPurchaseDto guestDto)
        {
            throw new NotImplementedException();
        }

        public Task<TicketDto> PurchaseTicketForUserAsync(int screeningId, int seatId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateTicketAsync(int ticketId)
        {
            throw new NotImplementedException();
        }
    }
}
