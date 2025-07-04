﻿using API.Entities;
using API.Extensions;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class MemberDTO
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public int Age { get; set; }
    public string? PhotoURL { get; set; }
    public string? KnownAs { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public List<PhotoDTO> Photos { get; set; } = [];
}
