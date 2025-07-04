﻿using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.ServiceContracts;

public interface IUserRepository
{
    void Update(AppUser user);
    //Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<MemberDTO?> GetMemberAsync(string username);
    Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
}
