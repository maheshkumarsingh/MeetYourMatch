﻿using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDTO>()
            .ForMember(dest => dest.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
            .ForMember(dest => dest.PhotoURL,
            o => o.MapFrom(source =>
            source.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Photo, PhotoDTO>();
        CreateMap<MemberUpdateDTO, AppUser>();
        CreateMap<RegisterDTO, AppUser>();
        CreateMap<string, DateOnly>().ConstructUsing(s => DateOnly.Parse(s));
        CreateMap<Message, MessageDTO>()
            .ForMember(d => d.SenderPhotoUrl,
                o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(d => d.RecipientPhotoUrl,
                o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }
}