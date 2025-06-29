using API.Data;
using API.ServiceContracts;

namespace API.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMessageRepository _messageRepository;
    private readonly ILikesRepository _likesRepository;
    private readonly IUserRepository _userRepository;
    private readonly DataContext _dataContext;

    public UnitOfWork(IMessageRepository messageRepository, ILikesRepository likesRepository, IUserRepository userRepository, DataContext dataContext)
    {
        _messageRepository = messageRepository;
        _likesRepository = likesRepository;
        _userRepository = userRepository;
        _dataContext = dataContext;
    }

    public IUserRepository UserRepository => _userRepository ;

    public IMessageRepository MessageRepository => _messageRepository;

    public ILikesRepository LikesRepository => _likesRepository;

    public async Task<bool> Complete()
    {
        return await _dataContext.SaveChangesAsync()>0;
    }

    public bool HashChanges()
    {
        return _dataContext.ChangeTracker.HasChanges();
    }
}
