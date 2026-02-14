using ChatGPTApp.Context;
using ChatGPTApp.Models;
using ChatGPTApp.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTApp.Repositories
{
    public class ChatRepository : IRepository<Chat>
    {
        private async Task<AppDbContext> CreateAppDbContextAsync()
        {
            return await Task.Run(() =>
            {
                return new AppDbContext();
            });
        }

        public async Task AddAsync(Chat entity)
        {
            try
            {
                using (var _appDbContext = await CreateAppDbContextAsync())
                {
                    await _appDbContext.Chats.AddAsync(entity);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<Chat> GetItemByConditionAsync(Expression<Func<Chat, bool>> predicate)
        {
            try
            {
                using (var _appDbContext = await CreateAppDbContextAsync())
                {
                    return await _appDbContext.Chats.Where(predicate).Include(x => x.Messages).FirstOrDefaultAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task RemoveAsync(string chatTitle)
        {
            try
            {
                using (var _appDbContext = await CreateAppDbContextAsync())
                {
                    var foundChat = await GetItemByConditionAsync(x => x.Title == chatTitle);
                    if (foundChat != null)
                    {
                        _appDbContext.Chats.Remove(foundChat);
                        await _appDbContext.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> SelectListByConditionAsync(Expression<Func<Chat, string>> predicate)
        {
            try
            {
                using (var _appDbContext = await CreateAppDbContextAsync())
                {
                    return await _appDbContext.Chats.OrderByDescending(x => x.Id).Select(predicate).ToListAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateAsync(Chat newChat)
        {
            try
            {
                using (var _appDbContext = await CreateAppDbContextAsync())
                {
                    _appDbContext.Chats.Update(newChat);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
