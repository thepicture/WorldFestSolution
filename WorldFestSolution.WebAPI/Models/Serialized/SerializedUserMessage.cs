using System;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedUserMessage
    {
        public SerializedUserMessage(UserMessage m)
        {
            Id = m.Id;
            SenderId = m.SenderId;
            ReceiverId = m.ReceiverId;
            Message = m.Message;
            PostDateTime = m.PostDateTime;

            Receiver = new SerializedUser(m.User);
            Sender = new SerializedUser(m.User1);
        }

        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime PostDateTime { get; set; }

        public virtual SerializedUser Receiver { get; set; }
        public virtual SerializedUser Sender { get; set; }
    }
}