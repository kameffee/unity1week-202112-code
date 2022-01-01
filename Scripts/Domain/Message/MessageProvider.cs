using System.Collections.Generic;

namespace Unity1week202112.Domain.Message
{
    /// <summary>
    /// メッセージのキューして保持する.
    /// </summary>
    public class MessageProvider
    {
        private Queue<MessageModel> _messageQueue = new Queue<MessageModel>();

        /// <summary>
        /// メッセージがまだあるか
        /// </summary>
        public bool ExistsMessage => _messageQueue.Count > 0;

        public void Queue(MessageModel message)
        {
            _messageQueue.Enqueue(message);
        }
        
        public void Queue(IEnumerable<MessageModel> messages)
        {
            foreach (var messageModel in messages)
            {
                _messageQueue.Enqueue(messageModel);
            }
        }

        public MessageModel Dequeue()
        {
            return _messageQueue.Dequeue();
        }

        public void Clear()
        {
            _messageQueue.Clear();
        }
    }
}
