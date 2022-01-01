using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using UniRx;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Map;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Presentation.UI;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Presentation.Title
{
    public class TitleScene : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject]
        private IScreenFader _fader;

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private TitleView _view;
        private readonly TextShake _textShake;
        private readonly Func<SettingsWindowPresenter> _settingsPresenter;
        private readonly SceneParameterContainer _sceneParameterContainer;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public TitleScene(TitleView view,
            TextShake textShake,
            Func<SettingsWindowPresenter> settingsPresenter,
            SceneParameterContainer sceneParameterContainer)
        {
            _view = view;
            _textShake = textShake;
            _settingsPresenter = settingsPresenter;
            _sceneParameterContainer = sceneParameterContainer;
        }

        public void Initialize()
        {
            _view.OnClickStart
                .Subscribe(async _ => await PlayGame())
                .AddTo(_disposable);

            _view.OnClickSettings
                .Subscribe(async _ => OpenSettings(default).Forget())
                .AddTo(_disposable);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _audioPlayer.Bgm.CrossFade(0, 1f);
            await _fader.FadeIn(1, cancellation);

            _textShake.Shake();
        }

        /// <summary>
        /// はじめる
        /// </summary>
        private async UniTask PlayGame()
        {
            _textShake.FadeOut();

            await UniTask.Delay(TimeSpan.FromSeconds(2f));

            await _fader.FadeOut();

            // 敵をセット

            _sceneParameterContainer.Push(SceneIndex.Title, new MapSceneParameter(new MapPoint(0)));
            await SceneManager.LoadSceneAsync((int)SceneIndex.Map);
        }

        public async UniTaskVoid OpenSettings(CancellationToken cancellationToken)
        {
            var settingsWindowPresenter = _settingsPresenter.Invoke();
            await settingsWindowPresenter.Open(cancellationToken);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
