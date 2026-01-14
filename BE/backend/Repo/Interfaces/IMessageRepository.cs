using backend.Domain;
using backend.Dtos;

namespace backend.Repo.Interfaces;

public interface IMessageRepository
{
    Task<Message> CreateMessageAsync(Message message);
    Task<List<Message>> GetConversationAsync(int userId, string userType, int otherUserId, string otherUserType);
    Task<List<ConversationDto>> GetConversationsAsync(int userId, string userType);
    Task<int> MarkMessagesAsReadAsync(int receiverId, string receiverType, int senderId, string senderType);
    Task<int> GetUnreadCountAsync(int userId, string userType);
    Task<Message?> GetMessageByIdAsync(int id);
}