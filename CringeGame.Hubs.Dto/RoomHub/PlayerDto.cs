namespace CringeGame.Hubs.Dto.RoomHub;

/// <summary>
/// Игрок.
/// </summary>
public class PlayerDto
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }
}