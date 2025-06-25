﻿using API.Entities;

namespace API.DTOs;

public class MessageDTO
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public required string SenderUserName { get; set; }
    public required string SenderPhotoUrl { get; set; }
    public int RecipientId { get; set; }
    public required string RecipientUserName { get; set; }
    public required string RecipientPhotoUrl { get; set; }
    public required string Content { get; set; }
    //properties like Id, MessageSent = CreatedAt.
    public DateTime? DateRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
