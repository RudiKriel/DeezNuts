﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using Common.DTOs;
using DeezNuts.Extenstions;
using Common.Models;
using AutoMapper;
using DAL.Helpers;

namespace DeezNuts.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messagesRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepository, IMessageRepository messagesRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO model)
        {
            var username = User.GetUserName();

            if (username == model.RecipientUsername.ToLower())
            {
                return BadRequest("We know you are lonelly, but try not to send messages to yourself");
            }

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(model.RecipientUsername);

            if (recipient  == null)
            {
                return NotFound();
            }

            var message = new Message()
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = model.Content
            };

            _messagesRepository.AddMessage(message);

            if (await _messagesRepository.SaveAllAsync())
            {
                return Ok(_mapper.Map<MessageDTO>(message));
            }

            return BadRequest("Damn your message got ignored and could not be sent");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUserName();

            var messages = await _messagesRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUserName();

            return Ok(await _messagesRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("delete-message/{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUserName();
            var message = await _messagesRepository.GetMessage(id);

            if (message.Sender.UserName != username && message.Recipient.UserName != username)
            {
                return Unauthorized();
            }

            if (message.Sender.UserName == username)
            {
                message.SenderDeleted = true;
            }

            if (message.Recipient.UserName == username)
            {
                message.RecipientDeleted = true;
            }

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _messagesRepository.DeleteMessage(message);
            }

            if (await _messagesRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Problem deleting message, you are stuck with it now");
        }
    }
}
