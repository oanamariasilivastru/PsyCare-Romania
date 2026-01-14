using backend.Dtos;
using backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PSYCare.Services;

namespace backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }
        
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto messageDto)
        {
            if (messageDto == null || string.IsNullOrWhiteSpace(messageDto.Content))
                return BadRequest("Message content is required");

            try
            {
                var createdMessage = await _messageService.SendMessageAsync(messageDto);
                return Ok(createdMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to send message", error = ex.Message });
            }
        }
        
        [HttpGet("conversation")]
        public async Task<ActionResult<List<MessageDto>>> GetConversation(
            [FromQuery] int userId,
            [FromQuery] string userType,
            [FromQuery] int otherUserId,
            [FromQuery] string otherUserType)
        {
            try
            {
                var messages = await _messageService.GetConversationAsync(userId, userType, otherUserId, otherUserType);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get conversation", error = ex.Message });
            }
        }
        
        [HttpGet("conversations")]
        public async Task<ActionResult<List<ConversationDto>>> GetConversations(
            [FromQuery] int userId,
            [FromQuery] string userType)
        {
            try
            {
                var conversations = await _messageService.GetConversationsAsync(userId, userType);
                return Ok(conversations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get conversations", error = ex.Message });
            }
        }
        
        [HttpPut("mark-as-read")]
        public async Task<IActionResult> MarkMessagesAsRead([FromBody] MarkAsReadDto markAsReadDto)
        {
            if (markAsReadDto == null)
                return BadRequest("Invalid data");

            try
            {
                var success = await _messageService.MarkMessagesAsReadAsync(markAsReadDto);

                if (!success)
                    return NotFound(new { message = "No unread messages found" });

                return Ok(new { message = "Messages marked as read" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to mark messages as read", error = ex.Message });
            }
        }
        
        [HttpGet("unread-count")]
        public async Task<ActionResult<int>> GetUnreadCount(
            [FromQuery] int userId,
            [FromQuery] string userType)
        {
            try
            {
                var count = await _messageService.GetUnreadCountAsync(userId, userType);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get unread count", error = ex.Message });
            }
        }
    }
}
