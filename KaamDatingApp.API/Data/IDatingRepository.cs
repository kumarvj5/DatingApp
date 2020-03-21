using System.Collections.Generic;
using System.Threading.Tasks;
using KaamDatingApp.API.Helpers;
using KaamDatingApp.API.Models;

namespace KaamDatingApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T:class;
         void Update<T>(T entity) where T:class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Message> GetMessage(int id);
         Task<IEnumerable<Message>> GetMessageThread(int id, int recepientId);
         Task<PagedList<Message>> GetMessagesGorUser(MessageParams messageParams);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);
         Task<Like> GetLike(int userId, int recepientId);
    }
}