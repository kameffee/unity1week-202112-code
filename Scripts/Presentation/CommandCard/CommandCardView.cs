using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kameffee.AudioPlayer;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Unity1week202112.Presentation.CommandCard
{
    public class CommandCardView : MonoBehaviour, ICommandCardView
    {
        [SerializeField]
        private RectTransform _root;

        [FormerlySerializedAs("_canvasGroup")]
        [SerializeField]
        private CanvasGroup _rootCanvas;

        [SerializeField]
        private CanvasGroup _cardCanvas;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _num;

        [SerializeField]
        private CanvasGroup _mask;

        [SerializeField]
        private CanvasGroup _lockRoot;

        [SerializeField]
        private CanvasGroup _lockUseRoot;

        [SerializeField]
        private TextMeshProUGUI _lockCount;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private CommandCardPerformer _performer;

        [Header("Audio")]
        [SerializeField]
        private AudioClip _selectedSeClip;

        public IObservable<Unit> OnClick => _button.OnClickAsObservable();

        public IObservable<Unit> OnRightClick => this.UpdateAsObservable()
            .Where(_ => _mouthOver)
            .Where(_ => Input.GetMouseButtonDown(1));

        public IObservable<Unit> OnLongPress => _onLongPress;
        private readonly Subject<Unit> _onLongPress = new Subject<Unit>();

        private bool _mouthOver;
        private RectTransform _rectTransform;
        private Vector2 _currentAnchorPosition;
        private CommandCardFeedback _feedBack;
        private float _longTapTime = 0.8f;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _mask.alpha = 1;
            _rootCanvas.alpha = 0;
            _cardCanvas.alpha = 0;

            _feedBack = GetComponent<CommandCardFeedback>();

            _button.OnPointerDownAsObservable()
                .Subscribe(data => OnDown(data).Forget());

            _button.OnPointerEnterAsObservable().Subscribe(_ => _mouthOver = true);
            _button.OnPointerExitAsObservable().Subscribe(_ => _mouthOver = false);
        }

        private CancellationTokenSource _onLongPressCancellation;

        private async UniTaskVoid OnDown(PointerEventData pointerEventData)
        {
            _onLongPressCancellation?.Cancel();
            _onLongPressCancellation = new CancellationTokenSource();
            _button.OnPointerUpAsObservable()
                .Subscribe(_ => _onLongPressCancellation.Cancel())
                .AddTo(_onLongPressCancellation.Token);

            await UniTask.Delay(TimeSpan.FromSeconds(_longTapTime), DelayType.Realtime,
                    cancellationToken: _onLongPressCancellation.Token)
                .WithCancellation(_onLongPressCancellation.Token);

            _onLongPressCancellation?.Cancel();
            _onLongPress.OnNext(Unit.Default);
        }

        public void SetImage(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public void SetNum(int num)
        {
            _num.SetText(num >= 1 ? num.ToString() : "");
        }

        public void SetPosition(Vector2 pos)
        {
            _rectTransform.anchoredPosition = pos;
            _currentAnchorPosition = pos;
        }

        public async UniTask PlayInAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_rootCanvas.DOFade(1, 0.3f));
            sequence.Join(_root.DOAnchorPosY(60f, 0.5f).From(true));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
        }

        public async UniTask PlayUseAnimation(CancellationToken cancellation)
        {
            await _performer.FadeOut(cancellation);
        }

        public async UniTask Move(Vector2 toPos)
        {
            _currentAnchorPosition = toPos;
            await _rectTransform.DOAnchorPos(toPos, 0.5f);
        }

        public async UniTask Show()
        {
            _cardCanvas.alpha = 1;
            var sequence = DOTween.Sequence();
            sequence.Append(_mask.DOFade(0, 1f));
        }

        public void Delete()
        {
            Destroy(gameObject);
        }

        public void SetSelectedState(bool isSelected, bool forceLastSibling = true)
        {
            if (isSelected)
            {
                AudioPlayer.Instance.Se.Play(_selectedSeClip);
            }
            
            // 一番手前に
            if (forceLastSibling)
            {
                _rectTransform.SetAsLastSibling();
            }

            Vector2 toPos = isSelected
                ?  new Vector2(0, 50f)
                : Vector2.zero;

            _rectTransform.DOAnchorPos(toPos, 0.3f).SetRelative().SetEase(Ease.OutSine);
        }

        public void SetSelectableState(bool selectable)
        {
            _lockRoot.DOFade(selectable ? 0 : 1f, 0.2f);
        }

        public void SetEnableFeedBack(bool isEnable)
        {
            _feedBack?.SetEnable(isEnable);
        }

        public void SetLockCount(int count)
        {
            _lockCount.SetText(count.ToString());
            _lockUseRoot.DOFade(count > 0 ? 1 : 0, 0.2f);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
            _onLongPress?.Dispose();
            _onLongPressCancellation?.Dispose();
        }
    }
}
