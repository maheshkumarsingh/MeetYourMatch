using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.ServiceContracts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class LikesRepository : ILikesRepository
{
    private readonly DataContext _datacontext;
    private readonly IMapper _mapper;

    public LikesRepository(DataContext datacontext, IMapper mapper)
    {
        _datacontext = datacontext;
        _mapper = mapper;
    }

    public void AddLike(UserLike like)
    {
        _datacontext.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
        _datacontext.Likes.Remove(like);
    }

    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
        return await _datacontext.Likes
            .Where(x => x.SourceUserId == currentUserId)
            .Select(x => x.TargetUserId)
            .ToListAsync();
    }

    public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await _datacontext.Likes.FindAsync(sourceUserId, targetUserId);
    }

    public async Task<PagedList<MemberDTO>> GetUserLikes(LikesParams likesParams)
    {
        var likes = _datacontext.Likes.AsQueryable();
        IQueryable<MemberDTO> query;
        switch (likesParams.Predicate)
        {
            case "liked":
                query = likes.Where(x => x.SourceUserId == likesParams.UserId)
                                            .Select(x => x.TargetUser)
                                            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);
                break;
            case "likedBy":
                query = likes.Where(x => x.TargetUserId == likesParams.UserId)
                                            .Select(x => x.SourceUser)
                                            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);
                break;
            default:
                var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);
                query = likes
                            .Where(x => x.TargetUserId == likesParams.UserId &&
                                likeIds.Contains(x.SourceUserId))
                            .Select(x => x.SourceUser)
                            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);
                break;
        }
        return await PagedList<MemberDTO>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<bool> SaveChanges()
    {
        return await _datacontext.SaveChangesAsync() > 0;
    }
}
