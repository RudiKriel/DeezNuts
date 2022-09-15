using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.DTOs;
using Common.Models;
using DAL.Context;
using DAL.Helpers;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.OrderByDescending(m => m.MessageSent)
                .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                .AsQueryable();

            messages = messageParams.Container switch
            {
                "Inbox" => messages.Where(u => u.RecipientUsername == messageParams.Username && !u.RecipientDeleted),
                "Outbox" => messages.Where(u => u.SenderUsername == messageParams.Username && !u.SenderDeleted),
                _ => messages.Where(u => u.RecipientUsername == messageParams.Username && !u.RecipientDeleted && u.DateRead == null)
            };

            return await PagedList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName, string recipientUsername)
        {
            var messages = await _context.Messages.Where(m => (m.Recipient.UserName == currentUserName && m.Sender.UserName == recipientUsername && !m.RecipientDeleted)
                        || (m.Recipient.UserName == recipientUsername && m.SenderUsername == currentUserName && !m.SenderDeleted))
                .OrderBy(m => m.MessageSent)
                .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUserName).ToList();

            if (unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }

            return messages;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups.Where(g => g.Connections.Any(c => c.ConnectionId == connectionId)).FirstOrDefaultAsync();
        }
    }
}
