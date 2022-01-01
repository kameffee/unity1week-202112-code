using Kameffee.AudioPlayer;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Deck;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Domain.User;
using Unity1week202112.Presentation.ScreenFade;
using Unity1week202112.Presentation.Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Installer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private ScreenFadeView _screenFadeViewPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<AudioListener>(Lifetime.Singleton);
            builder.RegisterComponentOnNewGameObject<EventSystem>(Lifetime.Singleton);

            // シーン間のデータ橋渡し
            builder.Register<SceneParameterContainer>(Lifetime.Singleton);

            // チュートリアル
            builder.Register<TutorialProvider>(Lifetime.Singleton);

            // 画面フェード
            builder.Register<ScreenFader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(_screenFadeViewPrefab, Lifetime.Singleton)
                .As<IScreenFadeView>()
                .AsSelf();
            builder.RegisterEntryPoint<ScreenFadePresenter>();

            builder.Register<PlayerDeckRepository>(Lifetime.Singleton);
            builder.Register<UserRepository>(Lifetime.Singleton);

            builder.RegisterInstance<AudioPlayer>(AudioPlayer.Instance);

            builder.RegisterBuildCallback(resolver =>
            {
                var audioPlayer = resolver.Resolve<AudioPlayer>();
                audioPlayer.Bgm.SetVolume(0.5f);
                audioPlayer.Se.SetVolume(0.5f);

                // 作っておく.
                DontDestroyOnLoad(resolver.Resolve<AudioListener>().gameObject);
                DontDestroyOnLoad(resolver.Resolve<EventSystem>().gameObject.AddComponent<StandaloneInputModule>());
                DontDestroyOnLoad(resolver.Resolve<ScreenFadeView>().gameObject);
            });
        }
    }
}
