using AutoMapper;
using CringeGame.Hubs.Dto.RoomHub;
using CringeGame.Models;

namespace CringeGame.Hubs;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<CreateRoomDto, Room>();
        CreateMap<Room, GetRoomDto>();
        CreateMap<Player, PlayerDto>();
        CreateMap<Game, GameStartDto>();
    }
}