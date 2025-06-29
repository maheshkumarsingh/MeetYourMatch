using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.ServiceContracts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDTO?> GetMemberAsync(string username)
    {
        return await context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
    {
        var query = context.Users
                        .AsQueryable();
                        
        query = query.Where(x => x.UserName.ToLower() != userParams.CurrentUserName!.ToLower());
        if(userParams.Gender is not null)
        {
            query = query.Where(x => x.Gender == userParams.Gender);
        }
        var minDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge - 1));

        query = query.Where(x => x.DateOfBirth >= minDOB && x.DateOfBirth <= maxDOB);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x=>x.UpdatedAt)
        };
        return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
                            .Include(x => x.Photos)
                            .FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());

    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
                            .Include(x => x.Photos)
                            .ToListAsync();
    }

    //public async Task<bool> SaveAllAsync()
    //{
    //    return await context.SaveChangesAsync() > 0;
    //}

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
