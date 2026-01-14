using backend.Domain;
using backend.Dtos;
using backend.Repo;
using backend.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PSYCare.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly PSYCareDbContext _context;

        public MessageRepository(PSYCareDbContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetConversationAsync(int userId, string userType, int otherUserId, string otherUserType)
        {
            return await _context.Messages
                .Where(m => 
                    (m.SenderId == userId && m.SenderType == userType && 
                     m.ReceiverId == otherUserId && m.ReceiverType == otherUserType) ||
                    (m.SenderId == otherUserId && m.SenderType == otherUserType && 
                     m.ReceiverId == userId && m.ReceiverType == userType))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<List<ConversationDto>> GetConversationsAsync(int userId, string userType)
        {
            var messages = await _context.Messages
                .Where(m => 
                    (m.SenderId == userId && m.SenderType == userType) ||
                    (m.ReceiverId == userId && m.ReceiverType == userType))
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
            
            var conversations = messages
                .GroupBy(m => 
                {
                    if (m.SenderId == userId && m.SenderType == userType)
                        return new { Id = m.ReceiverId, Type = m.ReceiverType };
                    else
                        return new { Id = m.SenderId, Type = m.SenderType };
                })
                .Select(g => new
                {
                    UserId = g.Key.Id,
                    UserType = g.Key.Type,
                    LastMessage = g.First(),
                    UnreadCount = g.Count(m => 
                        m.ReceiverId == userId && 
                        m.ReceiverType == userType && 
                        !m.IsRead)
                })
                .ToList();
            
            var conversationDtos = new List<ConversationDto>();

            foreach (var conv in conversations)
            {
                string userName = await GetUserNameAsync(conv.UserId, conv.UserType);

                conversationDtos.Add(new ConversationDto
                {
                    UserId = conv.UserId,
                    UserName = userName,
                    UserType = conv.UserType,
                    LastMessage = conv.LastMessage.Content,
                    LastMessageTime = conv.LastMessage.SentAt,
                    UnreadCount = conv.UnreadCount
                });
            }

            return conversationDtos.OrderByDescending(c => c.LastMessageTime).ToList();
        }

        public async Task<int> MarkMessagesAsReadAsync(int receiverId, string receiverType, int senderId, string senderType)
        {
            var unreadMessages = await _context.Messages
                .Where(m => 
                    m.SenderId == senderId && 
                    m.SenderType == senderType &&
                    m.ReceiverId == receiverId && 
                    m.ReceiverType == receiverType &&
                    !m.IsRead)
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId, string userType)
        {
            return await _context.Messages
                .Where(m => 
                    m.ReceiverId == userId && 
                    m.ReceiverType == userType && 
                    !m.IsRead)
                .CountAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        private async Task<string> GetUserNameAsync(int userId, string userType)
        {
            if (userType == "patient")
            {
                var patient = await _context.Patients.FindAsync(userId);
                return patient?.Name ?? "Unknown Patient";
            }
            else if (userType == "psychologist")
            {
                var psychologist = await _context.Psychologists.FindAsync(userId);
                return psychologist?.Name ?? "Unknown Psychologist";
            }
            return "Unknown User";
        }
    }
}