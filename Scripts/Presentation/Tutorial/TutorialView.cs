using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Unity1week202112.Presentation.Tutorial
{
    public interface ITutorialView : IDisposable
    {
        UniTask StartTutorial(CancellationToken cancellationToken);
    }

    public class TutorialView : MonoBehaviour, ITutorialView
    {
        [SerializeField]
        private CanvasGroup _background;

        [SerializeField]
        private CanvasGroup[] _pages;

        public int PageCount => _pages.Length;

        public IReadOnlyReactiveProperty<int> CurrentPage => _currentPage;
        private readonly IntReactiveProperty _currentPage = new IntReactiveProperty();

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var canvasGroup in _pages)
            {
                canvasGroup.alpha = 0;
            }

            _background.alpha = 0;
        }

        public async UniTask StartTutorial(CancellationToken cancellationToken)
        {
            await _background.DOFade(1, 0.2f).WithCancellation(cancellationToken);

            _currentPage.Value = 0;

            while (HasNext())
            {
                // 表示
                await _pages[_currentPage.Value].DOFade(1, 0.5f);

                // クリック待ち
                var input = UniTask.WaitUntil(() => Input.anyKeyDown || Input.GetMouseButtonDown(0),
                    cancellationToken: cancellationToken);
                await UniTask.WhenAny(input).WithCancellation(cancellationToken);

                // 非表示
                await _pages[_currentPage.Value].DOFade(0, 0.2f);

                Next();
            }

            _background.DOFade(0, 0.2f).WithCancellation(cancellationToken);
        }

        private void Next()
        {
            _currentPage.Value++;
        }

        private bool HasNext()
        {
            return _currentPage.Value < PageCount;
        }

        public void Dispose()
        {
            _currentPage?.Dispose();
            Destroy(gameObject);
        }
    }
}
