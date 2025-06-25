using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.ServiceContracts;

public interface IMessageRepository
{
    void AddMesage(Message message);
    void DeleteMesage(Message message);
    Task<Message?> GetMessage(int id);
    Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);
    Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName, string recipientUserName);
    Task<bool> SaveAllAsync();
}
