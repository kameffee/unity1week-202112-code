using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain.Message;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202112.Presentation.Talk
{
    public class MessagePresenter : IStartable
    {
        private readonly MessageProvider _messageProvider;
        private readonly TalkMessageScenario _talkMessageScenario;
        private readonly MessageWindow _window;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public MessagePresenter(MessageProvider messageProvider, TalkMessageScenario talkMessageScenario, MessageWindow window)
        {
            _messageProvider = messageProvider;
            _talkMessageScenario = talkMessageScenario;
            _window = window;
        }

        public void Start()
        {
            _talkMessageScenario.OnStart
                .Subscribe(_ => UniTask.Void(async () => await StartMessage()))
                .AddTo(_disposable);
        }

        private async UniTask StartMessage()
        {
            // 開く
            await _window.Open();

            while (_messageProvider.ExistsMessage)
            {
                // 取り出す
                MessageModel message = _messageProvider.Dequeue();
                await _window.StartMessage(message.Message);
                
                // クリック待ち
                await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.anyKeyDown);
            }
            
            // 閉じる
            await _window.Close();
            
            // 完了を知らせる
            _talkMessageScenario.Complete();
        }
    }
}
