namespace Unity1week202112.Domain.Message
{
    /// <summary>
    /// メッセージ
    /// </summary>
    public class MessageModel
    {
        public int GroupId { get; }

        public int Id { get; }

        public string Message { get; }

        public MessageModel(int groupId, int id, string message)
        {
            GroupId = groupId;
            Id = id;
            Message = message;
        }
    }
}
