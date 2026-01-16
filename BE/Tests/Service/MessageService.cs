using backend.Domain;
using backend.Dtos;
using backend.Repo.Interfaces;
using PSYCare.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class MessageServiceTests
{
    [Fact]
    public async Task SendMessageAsync_ShouldCreateMessage()
    {
        // Arrange
        var mockMessageRepo = new Mock<IMessageRepository>();
        var mockPatientRepo = new Mock<IPatientRepository>();
        var mockPsychologistRepo = new Mock<IPsychologistRepository>();

        var message = new Message
        {
            Id = 1,
            SenderId = 1,
            SenderType = "patient",
            ReceiverId = 2,
            ReceiverType = "psychologist",
            Content = "Hello",
            SentAt = DateTime.UtcNow
        };

        mockMessageRepo
            .Setup(x => x.CreateMessageAsync(It.IsAny<Message>()))
            .ReturnsAsync(message);

        mockPatientRepo.Setup(x => x.GetPatientById(1))
            .Returns(new Patient { Id = 1, Name = "Ana" });

        mockPsychologistRepo.Setup(x => x.GetPsychologistById(2))
            .Returns(new Psychologist { Id = 2, Name = "Dr. Popescu" });

        var service = new MessageService(mockMessageRepo.Object, mockPatientRepo.Object, mockPsychologistRepo.Object);

        var dto = new SendMessageDto
        {
            SenderId = 1,
            SenderType = "patient",
            ReceiverId = 2,
            ReceiverType = "psychologist",
            Content = "Hello"
        };

        // Act
        var result = await service.SendMessageAsync(dto);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Ana", result.SenderName);
        Assert.Equal("Dr. Popescu", result.ReceiverName);
        Assert.Equal("Hello", result.Content);
    }

    [Fact]
    public async Task GetConversationAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var mockMessageRepo = new Mock<IMessageRepository>();
        var mockPatientRepo = new Mock<IPatientRepository>();
        var mockPsychologistRepo = new Mock<IPsychologistRepository>();

        var messages = new List<Message>
        {
            new Message
            {
                Id = 1,
                SenderId = 1,
                SenderType = "patient",
                ReceiverId = 2,
                ReceiverType = "psychologist",
                Content = "Hi",
                SentAt = DateTime.UtcNow
            }
        };

        mockMessageRepo
            .Setup(x => x.GetConversationAsync(1, "patient", 2, "psychologist"))
            .ReturnsAsync(messages);

        mockPatientRepo.Setup(x => x.GetPatientById(1))
            .Returns(new Patient { Id = 1, Name = "Ana" });

        mockPsychologistRepo.Setup(x => x.GetPsychologistById(2))
            .Returns(new Psychologist { Id = 2, Name = "Dr. Popescu" });

        var service = new MessageService(mockMessageRepo.Object, mockPatientRepo.Object, mockPsychologistRepo.Object);

        // Act
        var result = await service.GetConversationAsync(1, "patient", 2, "psychologist");

        // Assert
        Assert.Single(result);
        Assert.Equal("Ana", result[0].SenderName);
        Assert.Equal("Dr. Popescu", result[0].ReceiverName);
        Assert.Equal("Hi", result[0].Content);
    }

    [Fact]
    public async Task MarkMessagesAsReadAsync_ShouldReturnTrue_WhenCountGreaterThanZero()
    {
        // Arrange
        var mockMessageRepo = new Mock<IMessageRepository>();
        var mockPatientRepo = new Mock<IPatientRepository>();
        var mockPsychologistRepo = new Mock<IPsychologistRepository>();

        mockMessageRepo
            .Setup(x => x.MarkMessagesAsReadAsync(1, "patient", 2, "psychologist"))
            .ReturnsAsync(3); // 3 messages marked as read

        var service = new MessageService(mockMessageRepo.Object, mockPatientRepo.Object, mockPsychologistRepo.Object);

        var dto = new MarkAsReadDto
        {
            UserId = 1,
            UserType = "patient",
            OtherUserId = 2,
            OtherUserType = "psychologist"
        };

        // Act
        var result = await service.MarkMessagesAsReadAsync(dto);

        // Assert
        Assert.True(result);
    }
}
