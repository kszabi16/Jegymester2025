using AutoMapper;
using Jegymester.DataContext.Entities;
using Jegymester.DataContext.Dtos;

namespace Jegymester.DataContext
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity to DTO mappings
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<MovieCreateDto, Movie>();
            CreateMap<MovieUpdateDto, Movie>();

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, UpdateUserRolesDto>().ReverseMap();
            CreateMap<Screening, ScreeningDto>().ReverseMap();
            CreateMap<Screening, ScreeningCreateDto>().ReverseMap();
            CreateMap<Screening, ScreeningUpdateDto>().ReverseMap();
            CreateMap<Seat, SeatDto>().ReverseMap();
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<Ticket, UserTicketPurchaseDto>().ReverseMap();
            CreateMap<Ticket, GuestTicketPurchaseDto>().ReverseMap();
            CreateMap<Ticket, CashierTicketPurchaseDto>().ReverseMap();
            CreateMap<Ticket, ValidateTicketDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
        }
    }
}

