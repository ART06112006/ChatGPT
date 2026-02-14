using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI;
using OpenAI.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatGPTApp.Models;

namespace ChatGPTApp.Services
{
    public class APIService
    {
        public string APIKey { get; set; }
        public Action<string> ExceptionMessage { get; set; }

        public async Task<string?> GetResponseFromChatGPTAsync(List<Message> messagesHistory)
        {
            //var openAiService = new OpenAIService(new OpenAiOptions()
            //{
            //    ApiKey = APIKey
            //});

            //var messages = new List<ChatMessage>();
            //foreach (var message in messagesHistory)
            //{
            //    if (message.Author == MessageAuthor.User)
            //    {
            //        messages.Add(ChatMessage.FromUser(message.Text));
            //    }
            //    else if (message.Author == MessageAuthor.GPT)
            //    {
            //        messages.Add(ChatMessage.FromAssistant(message.Text));
            //    }
            //    else if (message.Author == MessageAuthor.Sys)
            //    {
            //        messages.Add(ChatMessage.FromSystem(message.Text));
            //    }
            //}

            //var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            //{
            //    Messages = messages,
            //    Model = OpenAI.ObjectModels.Models.ChatGpt3_5Turbo,
            //    MaxTokens = 50//optional
            //});

            //if (completionResult.Successful)
            //{
            //    return completionResult.Choices.First().Message.Content;
            //}
            //else
            //{
            //    ExceptionMessage?.Invoke(completionResult.Error.Message);
            //    return null;
            //}
            return "GPT Response";
        }
    }
}
