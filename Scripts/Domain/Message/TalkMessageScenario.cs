using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Unity1week202112.Domain.Message
{
    public class TalkMessageScenario
    {
        private readonly MessageFactory _messageFactory;
        private readonly MessageProvider _messageProvider;

        public IObservable<Unit> OnStart => _onStart;
        private readonly Subject<Unit> _onStart = new Subject<Unit>();

        public IObservable<Unit> OnEnd => _onEnd;
        private readonly Subject<Unit> _onEnd = new Subject<Unit>();

        public TalkMessageScenario(
            MessageFactory messageFactory, 
            MessageProvider messageProvider)
        {
            _messageFactory = messageFactory;
            _messageProvider = messageProvider;
        }

        public async UniTask StartTalkScenario(int id, CancellationToken cancellation)
        {
            // IDに該当するシナリオを取ってくる
            var messageModels= _messageFactory.GetMessages(id);
            
            // 積む
            _messageProvider.Queue(messageModels);

            _onStart.OnNext(Unit.Default);

            await OnEnd.ToUniTask(true, cancellation);
        }

        /// <summary>
        /// 終了した時に呼ぶ
        /// </summary>
        public void Complete()
        {
            _onEnd.OnNext(Unit.Default);
        }
    }
}
