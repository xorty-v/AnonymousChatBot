﻿namespace AnonymousChatBot.Domain.Entities;

public class Queue
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }
}