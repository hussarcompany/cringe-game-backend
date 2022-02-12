using AutoMapper;
using CringeGame.Hubs.Dto.GameHub;
using CringeGame.Hubs.Dto.RoomHub;
using CringeGame.Models;
using PlayerDto = CringeGame.Hubs.Dto.RoomHub.PlayerDto;

namespace CringeGame.Hubs;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<CreateRoomDto, Room>();
        CreateMap<Room, RoomDto>();
        CreateMap<Player, PlayerDto>();
        CreateMap<Player, GamePlayerDto>();
        CreateMap<Game, RoomGameDto>();
        CreateMap<Game, GameDto>();
        CreateMap<MemeCard, MemeCardDto>();
    }
}