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

            CreateMap<Role, RoleDto>();
            CreateMap<Role, UpdateUserRolesDto>().ReverseMap();

            CreateMap<Screening, ScreeningDto>();
            CreateMap<ScreeningCreateDto, Screening>();
            CreateMap<Screening, ScreeningUpdateDto>().ReverseMap();

            CreateMap<Seat, SeatDto>().ReverseMap();
            CreateMap<SeatCreateDto, Seat>();
            CreateMap<SeatUpdateDto, Seat>();

            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<RoomCreateDto, Room>();
            CreateMap<RoomUpdateDto, Room>();

            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<TicketCreateDto, Ticket>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Ticket, CashierTicketPurchaseDto>().ReverseMap();
            CreateMap<Ticket, ValidateTicketDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterUserDto, User>();
            CreateMap<RegisterWithRolesDto, User>();
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            


        }
    }
}

