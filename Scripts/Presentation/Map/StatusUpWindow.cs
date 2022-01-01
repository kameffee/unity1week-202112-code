using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kameffee.AudioPlayer;
using TMPro;
using Unity1week202112.Domain;
using Unity1week202112.Presentation.UI;
using UnityEngine;

namespace Unity1week202112.Presentation.Map
{
    public class StatusUpWindow : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private GameObject _winMessage;

        [SerializeField]
        private GameObject _loseMessage;

        [SerializeField]
        private TextMeshProUGUI _beforeValue;

        [SerializeField]
        private TextMeshProUGUI _afterValue;

        [SerializeField]
        private TextMeshProUGUI _addValue;

        [Header("Audio")]
        [SerializeField]
        private AudioClip _openSe;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }

        public void Initialize(bool isWin)
        {
            _winMessage.SetActive(isWin);
            _loseMessage.SetActive(!isWin);
        }

        public async UniTask Open(StatusChange statusChange, CancellationToken cancellationToken)
        {
            AudioPlayer.Instance.Se.Play(_openSe);

            _beforeValue.SetText(statusChange.Before.ToString());
            _afterValue.SetText(statusChange.After.ToString());
            var operate = statusChange.Offset > 0 ? "+" : "-";
            _addValue.SetText($"{operate}{statusChange.Offset}");

            await _canvasGroup.DOFade(1, 0.2f)
                .WithCancellation(cancellationToken);
        }

        public async UniTask Close(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(0, 0.5f)
                .WithCancellation(cancellationToken);
        }
    }
}
