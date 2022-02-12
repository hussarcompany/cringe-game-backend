using System.Text.Json.Serialization;

namespace CringeGame.Hubs.Dto.GameHub;

public class GameDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameStateDto State { get; set; }
    
    public List<GamePlayerDto> Players { get; set; }
    
    public string Statement { get; set; }
    
    public uint CurrentRound { get; set; }
    
    public uint MaxRounds { get; set; }
    
    public List<MemeCardDto> Hand { get; set; }
}