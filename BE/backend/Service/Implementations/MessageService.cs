
using backend.Domain;
using backend.Dtos;
using backend.Repo.Interfaces;

namespace PSYCare.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPsychologistRepository _psychologistRepository;

        public MessageService(
            IMessageRepository messageRepository,
            IPatientRepository patientRepository,
            IPsychologistRepository psychologistRepository)
        {
            _messageRepository = messageRepository;
            _patientRepository = patientRepository;
            _psychologistRepository = psychologistRepository;
        }

        public async Task<MessageDto> SendMessageAsync(SendMessageDto sendMessageDto)
        {
            var message = new Message
            {
                SenderId = sendMessageDto.SenderId,
                SenderType = sendMessageDto.SenderType,
                ReceiverId = sendMessageDto.ReceiverId,
                ReceiverType = sendMessageDto.ReceiverType,
                Content = sendMessageDto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            var createdMessage = await _messageRepository.CreateMessageAsync(message);

            return await MapToMessageDtoAsync(createdMessage);
        }

        public async Task<List<MessageDto>> GetConversationAsync(
            int userId, 
            string userType, 
            int otherUserId, 
            string otherUserType)
        {
            var messages = await _messageRepository.GetConversationAsync(
                userId, userType, otherUserId, otherUserType);

            var messageDtos = new List<MessageDto>();

            foreach (var message in messages)
            {
                messageDtos.Add(await MapToMessageDtoAsync(message));
            }

            return messageDtos;
        }

        public async Task<List<ConversationDto>> GetConversationsAsync(int userId, string userType)
        {
            return await _messageRepository.GetConversationsAsync(userId, userType);
        }

        public async Task<bool> MarkMessagesAsReadAsync(MarkAsReadDto markAsReadDto)
        {
            var count = await _messageRepository.MarkMessagesAsReadAsync(
                markAsReadDto.UserId,
                markAsReadDto.UserType,
                markAsReadDto.OtherUserId,
                markAsReadDto.OtherUserType);

            return count > 0;
        }

        public async Task<int> GetUnreadCountAsync(int userId, string userType)
        {
            return await _messageRepository.GetUnreadCountAsync(userId, userType);
        }

        private async Task<MessageDto> MapToMessageDtoAsync(Message message)
        {
            var senderName = await GetUserNameAsync(message.SenderId, message.SenderType);
            var receiverName = await GetUserNameAsync(message.ReceiverId, message.ReceiverType);

            return new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderName = senderName,
                SenderType = message.SenderType,
                ReceiverId = message.ReceiverId,
                ReceiverName = receiverName,
                ReceiverType = message.ReceiverType,
                Content = message.Content,
                SentAt = message.SentAt,
                ReadAt = message.ReadAt,
                IsRead = message.IsRead
            };
        }

        private async Task<string> GetUserNameAsync(int userId, string userType)
        {
            if (userType == "patient")
            {
                var patient = _patientRepository.GetPatientById(userId);
                return patient?.Name ?? "Unknown Patient";
            }
            else if (userType == "psychologist")
            {
                var psychologist = _psychologistRepository.GetPsychologistById(userId);
                return psychologist?.Name ?? "Unknown Psychologist";
            }
            return "Unknown User";
        }
    }
}