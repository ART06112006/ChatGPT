using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTApp.Models
{
    public enum MessageAuthor
    {
        User,
        GPT,
        Sys
    }

    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public MessageAuthor Author { get; set; }
        public string Text { get; set; }
    }
}
