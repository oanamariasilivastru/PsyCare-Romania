using backend.Dtos;

namespace PSYCare.Services
{
    public interface IMessageService
    {
        Task<MessageDto> SendMessageAsync(SendMessageDto sendMessageDto);
        Task<List<MessageDto>> GetConversationAsync(int userId, string userType, int otherUserId, string otherUserType);
        Task<List<ConversationDto>> GetConversationsAsync(int userId, string userType);
        Task<bool> MarkMessagesAsReadAsync(MarkAsReadDto markAsReadDto);
        Task<int> GetUnreadCountAsync(int userId, string userType);
    }
}