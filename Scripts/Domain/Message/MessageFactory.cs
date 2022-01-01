using System.Collections.Generic;
using System.Linq;
using Unity1week202112.Data.Master;
using UnityEngine;

namespace Unity1week202112.Domain.Message
{
    /// <summary>
    /// メッセージ生成
    /// </summary>
    public class MessageFactory
    {
        private List<MessageModel> _messagePool;

        public MessageFactory()
        {
            // ロード
            var data = Resources.Load<ScenarioMaster>("MasterData/ScenarioMaster");
            _messagePool = data.Data.Select(scenarioData =>
            {
                return new MessageModel(
                    scenarioData.ScenarioKey,
                    scenarioData.No,
                    scenarioData.Text.Replace(@"\n", "\n"));
            }).ToList();
        }

        public IEnumerable<MessageModel> GetMessages(int groupId)
        {
            return _messagePool.Where(message => message.GroupId == groupId);
        }
    }
}
