using ChatGPTApp.Models;
using ChatGPTApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTApp.Services
{
    public class ChatService
    {
        private readonly ChatRepository _chatRepository;
        
        public ChatService(ChatRepository chatRepository) 
        {
            _chatRepository = chatRepository;
        }

        public async Task<List<string>> GetAllChatTitlesAsync()
        {
            return new List<string>(await _chatRepository.SelectListByConditionAsync(x => x.Title));
        }

        public async Task<Chat> GetChatAsync(string chatTitle)
        {
            return await _chatRepository.GetItemByConditionAsync(x => x.Title == chatTitle);
        }

        public async Task AddChatAsync(Chat chat)
        {
            await _chatRepository.AddAsync(chat);
        }
        public async Task RemoveChatAsync(string chatTitle)
        {
            await _chatRepository.RemoveAsync(chatTitle);
        }

        public async Task UpdateChatAsync(Chat newChat)
        {
            await _chatRepository.UpdateAsync(newChat);
        }
    }
}
