using backend.Domain;
using backend.Dtos;
using backend.Repo;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PSYCare.Repositories;

public class MessageRepositoryTests
{
    private PSYCareDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PSYCareDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PSYCareDbContext(options);
    }

    [Fact]
    public async Task CreateMessageAsync_ShouldPersistMessage()
    {
        var context = GetDbContext();
        var repo = new MessageRepository(context);

        var message = new Message
        {
            SenderId = 1,
            SenderType = "patient",
            ReceiverId = 2,
            ReceiverType = "psychologist",
            Content = "Hello",
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        await repo.CreateMessageAsync(message);

        Assert.Single(context.Messages);
    }

    [Fact]
    public async Task GetConversationAsync_ShouldReturnOrderedConversation()
    {
        var context = GetDbContext();
        context.Messages.AddRange(
            new Message
            {
                SenderId = 1,
                SenderType = "patient",
                ReceiverId = 2,
                ReceiverType = "psychologist",
                Content = "First",
                SentAt = new DateTime(2025, 1, 1)
            },
            new Message
            {
                SenderId = 2,
                SenderType = "psychologist",
                ReceiverId = 1,
                ReceiverType = "patient",
                Content = "Second",
                SentAt = new DateTime(2025, 1, 2)
            }
        );
        await context.SaveChangesAsync();

        var repo = new MessageRepository(context);

        var result = await repo.GetConversationAsync(1, "patient", 2, "psychologist");

        Assert.Equal(2, result.Count);
        Assert.Equal("First", result[0].Content);
        Assert.Equal("Second", result[1].Content);
    }

    [Fact]
    public async Task MarkMessagesAsReadAsync_ShouldUpdateUnreadMessages()
    {
        var context = GetDbContext();
        context.Messages.Add(new Message
        {
            SenderId = 2,
            SenderType = "psychologist",
            ReceiverId = 1,
            ReceiverType = "patient",
            Content = "Unread",
            SentAt = DateTime.UtcNow,
            IsRead = false
        });
        await context.SaveChangesAsync();

        var repo = new MessageRepository(context);

        var updated = await repo.MarkMessagesAsReadAsync(1, "patient", 2, "psychologist");

        Assert.Equal(1, updated);
        Assert.True(context.Messages.First().IsRead);
    }
}
