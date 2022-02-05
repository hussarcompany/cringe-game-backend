namespace CringeGame.Hubs.Dto.RoomHub;

/// <summary>
/// Игровая комната.
/// </summary>
public class GetRoomDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Максимальное количество игроков.
    /// </summary>
    public int PlayersCount { get; set; }
    
    /// <summary>
    /// Текущее количество игроков.
    /// </summary>
    public int PlayersCountCurrent { get; set; }

    /// <summary>
    /// Состояние.
    /// </summary>
    public RoomState State { get; set; }
    
    /// <summary>
    /// Список игроков.
    /// </summary>
    public List<PlayerDto> Players { get; set; }
}