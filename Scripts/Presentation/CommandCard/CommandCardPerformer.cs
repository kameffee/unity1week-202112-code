using System;
using System.Linq;
using System.Threading;
using Coffee.UIEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Unity1week202112.Presentation.CommandCard
{
    public class CommandCardPerformer : MonoBehaviour
    {
        [SerializeField]
        private UIDissolve[] _dissolveList;

        public async UniTask FadeOut(CancellationToken cancellation)
        {
            var duration = _dissolveList.First().effectPlayer.duration;
            foreach (var uiDissolve in _dissolveList)
            {
                uiDissolve.Play();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: cancellation)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }
    }
}
