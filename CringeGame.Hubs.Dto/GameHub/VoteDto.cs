﻿namespace CringeGame.Hubs.Dto.GameHub;

public class VoteDto
{
    public Guid GameId { get; set; }
    
    public Guid UserId { get; set; }
}