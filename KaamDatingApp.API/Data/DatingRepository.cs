using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaamDatingApp.API.Helpers;
using KaamDatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KaamDatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recepientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u =>u.LikerId == userId && u.LikeeId == recepientId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId==userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p =>p.Id == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id==id);
            return(user);
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users =  _context.Users.Include(p => p.Photos).OrderByDescending(c=>c.LastActive).AsQueryable();
            users = users.Where(c=>c.Id != userParams.UserId);
            users = users.Where(c=>c.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(c=> userLikers.Contains(c.Id));
            }
            if (userParams.Likees)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(c=> userLikers.Contains(c.Id));
            }
            
            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge-1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(c=>c.DateOfBirth >= minDob && c.DateOfBirth <= maxDob);

            }
            
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                      break;
                    default :
                         users = users.OrderByDescending(c=>c.LastActive);
                      break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await _context.Users.Include(c=>c.Likers)
                                         .Include(c=>c.Likees)
                                         .FirstOrDefaultAsync(c=>c.Id == id);

            if (likers)
            {
                return user.Likers.Where(c=>c.LikeeId==id).Select(c=>c.LikerId);
            }
            else
            {
                return user.Likers.Where(c=>c.LikerId==id).Select(c=>c.LikeeId);
            }                                         
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);

        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.SingleOrDefaultAsync(c=>c.Id == id);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int id, int recepientId)
        {
            var messages = await _context.Messages.Include(c=>c.Sender).ThenInclude(c=>c.Photos)
                                                  .Include(c=>c.Recipient).ThenInclude(c=>c.Photos)
                                                  .Where(c=>c.RecipientId == id && c.RecipientDeleted==false && c.SenderId == recepientId ||
                                                        c.RecipientId == recepientId && c.SenderId == id && c.SenderDeleted==false)
                                                  .OrderByDescending(c=>c.MessageSent)
                                                  .ToListAsync();
                                                  
            return messages;
        }

        public async Task<PagedList<Message>> GetMessagesGorUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(c=>c.Sender).ThenInclude(c=>c.Photos)
                                                  .Include(c=>c.Recipient).ThenInclude(c=>c.Photos)
                                                  .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(c=>c.RecipientId == messageParams.UserId && c.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(c=>c.SenderId == messageParams.UserId && c.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(c=>c.RecipientId == messageParams.UserId && c.IsRead == false && c.RecipientDeleted == false);
                    break;
            }

            //messages = messages.OrderByDescending(c=>c.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }
    }
}