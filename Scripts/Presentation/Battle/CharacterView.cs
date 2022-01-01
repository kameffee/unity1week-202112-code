using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kameffee.AudioPlayer;
using UnityEngine;

namespace Unity1week202112.Presentation
{
    /// <summary>
    /// キャラ表示
    /// </summary>
    public class CharacterView : MonoBehaviour, IDamageFeedbackView
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private Transform _root;

        [SerializeField]
        private AudioClip _damegeClip;

        /// <summary>
        /// ダメージ演出
        /// </summary>
        /// <param name="cancellation"></param>
        public async UniTask Damage(CancellationToken cancellation = default)
        {
            AudioPlayer.Instance.Se.Play(_damegeClip);

            await _root.DOShakePosition(0.5f, 1, 20)
                .WithCancellation(cancellation);
        }
    }
}
