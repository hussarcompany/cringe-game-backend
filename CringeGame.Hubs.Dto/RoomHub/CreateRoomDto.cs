namespace CringeGame.Hubs.Dto.RoomHub;

/// <summary>
/// Игровая комната.
/// </summary>
public class CreateRoomDto
{
    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Максимальное количество игроков.
    /// </summary>
    public uint PlayersCount { get; set; }
}