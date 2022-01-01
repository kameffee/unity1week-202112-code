using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kameffee.AudioPlayer;
using UnityEngine;

namespace Unity1week202112.Presentation.Trade
{
    /// <summary>
    /// カード交換View
    /// </summary>
    public class CardTradeWindow : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [Header("Get")]
        [SerializeField]
        private CanvasGroup _getCardsCanvasGroup;

        [SerializeField]
        private Transform _selectGetCardHolder;

        [Header("Release")]
        [SerializeField]
        private CanvasGroup _releaseCardsCanvasGroup;

        [SerializeField]
        private Transform _selectReleaseCardHolder;

        [Header("Audio")]
        [SerializeField]
        private AudioClip _openSe;

        public Transform SelectGetCardHolder => _selectGetCardHolder;
        public Transform SelectReleaseCardHolder => _selectReleaseCardHolder;


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _canvasGroup.alpha = 0;
            _getCardsCanvasGroup.alpha = 0;
            _releaseCardsCanvasGroup.alpha = 0;
        }

        public async UniTask Open(CancellationToken cancellation)
        {
            await _canvasGroup.DOFade(1, .5f)
                .WithCancellation(cancellation);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public async UniTask Close(CancellationToken cancellation)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            await _canvasGroup.DOFade(0, .5f)
                .WithCancellation(cancellation);
        }

        public async UniTask OpenGetCards(CancellationToken cancellation)
        {
            await _getCardsCanvasGroup.DOFade(1, .2f)
                .WithCancellation(cancellation);

            AudioPlayer.Instance.Se.Play(_openSe);
        }

        public async UniTask CloseGetCards(CancellationToken cancellation)
        {
            await _getCardsCanvasGroup.DOFade(0, .2f)
                .WithCancellation(cancellation);
        }

        public async UniTask OpenReleaseCards(CancellationToken cancellation)
        {
            await _releaseCardsCanvasGroup.DOFade(1, .2f)
                .WithCancellation(cancellation);

            AudioPlayer.Instance.Se.Play(_openSe);
        }

        public async UniTask CloseReleaseCards(CancellationToken cancellation)
        {
            await _releaseCardsCanvasGroup.DOFade(0, .2f)
                .WithCancellation(cancellation);
        }
    }
}
