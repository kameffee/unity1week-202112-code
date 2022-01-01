using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using Unity1week202112.Domain.Message;
using UnityEngine;
using VContainer;

namespace Unity1week202112.Presentation
{
    public class EndingLoop
    {
        [Inject]
        private readonly AudioPlayer _audioPlayer; 
        private readonly TalkMessageScenario _talkMessageScenario;

        public EndingLoop(
            TalkMessageScenario talkMessageScenario)
        {
            _talkMessageScenario = talkMessageScenario;
        }

        public async UniTask Start(CancellationToken cancellation)
        {
            _audioPlayer.Bgm.CrossFade(2, 2f);
            await _talkMessageScenario.StartTalkScenario(1000, cancellation);
        }
    }
}
