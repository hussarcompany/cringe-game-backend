namespace CringeGame.Hubs.Dto.RoomHub;

/// <summary>
/// Игрок.
/// </summary>
public class PlayerDto
{
    public string ConnectionId { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }
}