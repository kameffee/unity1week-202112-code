using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Message;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Presentation;
using Unity1week202112.Presentation.CommandCard;
using Unity1week202112.Presentation.EffectDetail;
using Unity1week202112.Presentation.Talk;
using Unity1week202112.Presentation.Tutorial;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Installer
{
    public class InGameLifetimeScope : LifetimeScope
    {
        [Header("Notice")]
        [SerializeField]
        private RectTransform _noriceHolder;

        [SerializeField]
        private BattleStartNoticeView _battleStartNoticePrefab;

        [SerializeField]
        private BattleWinNoticeView _battleWinNoticePrefab;

        [SerializeField]
        private BattleLoseNoticeView _battleLoseNoticePrefab;

        [SerializeField]
        private CommandEffectDetailView _effectDetailViewPrefab;

        [Header("Cards")]
        [SerializeField]
        private Transform _cardHolder;

        [SerializeField]
        private CommandCardView _cardPrefab;

        [SerializeField]
        private MessageWindow _messageWindowPrefab;

        [Header("Debug")]
        [SerializeField]
        private InGameParameter _debugParameter;

        protected override void Configure(IContainerBuilder builder)
        {
            // キャラ生成
            builder.Register<BattleCharacterPrefabProvider>(Lifetime.Singleton);

            // カード生成
            builder.Register<CommandEffectIconProvider>(Lifetime.Singleton);
            builder.RegisterFactory<ICommandCardView>(
                resolver => { return () => Instantiate(_cardPrefab, _cardHolder); },
                Lifetime.Scoped);

            // フェーズ管理
            builder.Register<BattleStatus>(Lifetime.Singleton)
                .As<ICardSelectPhaseHandler>();

            builder.Register<CardSelectScenario>(Lifetime.Transient)
                .As<IWaitCardSelect>();

            builder.Register<BattlePlayerContainer>(Lifetime.Singleton);

            // 演出系

            // 開始演出
            builder.RegisterComponentInNewPrefab(_battleStartNoticePrefab, Lifetime.Singleton)
                .UnderTransform(_noriceHolder)
                .As<IBattleStartPerformer>()
                .AsSelf();

            // 勝利
            builder.RegisterComponentInNewPrefab(_battleWinNoticePrefab, Lifetime.Singleton)
                .UnderTransform(_noriceHolder)
                .As<IBattleWinPerformer>()
                .AsSelf();

            // 敗北
            builder.RegisterComponentInNewPrefab(_battleLoseNoticePrefab, Lifetime.Singleton)
                .UnderTransform(_noriceHolder)
                .As<IBattleLosePerformer>()
                .AsSelf();

            // コマンドデータ
            builder.Register<CommandEffectFactory>(Lifetime.Singleton);
            // 詳細ダイアログ
            builder.RegisterFactory<int, CommandEffectDetailModel>(resolver =>
            {
                return effectType =>
                {
                    var model = new CommandEffectDetailModel(effectType,
                        resolver.Resolve<CommandEffectFactory>(),
                        resolver.Resolve<CommandEffectIconProvider>());

                    var presenter = new CommandEffectDetailPresenter(
                        model,
                        Instantiate(_effectDetailViewPrefab, _noriceHolder));
                    presenter.Initialize();
                    return model;
                };
            }, Lifetime.Singleton);

            // メッセージ
            builder.RegisterComponentInNewPrefab<MessageWindow>(_messageWindowPrefab, Lifetime.Singleton)
                .UnderTransform(_noriceHolder)
                .As<IMessageWindow>()
                .AsSelf();

            builder.Register<MessageFactory>(Lifetime.Scoped);
            builder.Register<MessageProvider>(Lifetime.Scoped);
            builder.Register<TalkMessageScenario>(Lifetime.Scoped);
            builder.RegisterEntryPoint<MessagePresenter>();

            // ターン
            builder.Register<GameTurn>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<TurnView>().As<ITurnView>().AsSelf();
            builder.RegisterEntryPoint<GameTurnPresenter>();

            // ゲームループ
            builder.Register<BattleLoop>(Lifetime.Scoped);
            builder.RegisterEntryPoint<GameLoop>();

            builder.RegisterFactory<int, TutorialPresenter>(resolver =>
            {
                var provider = resolver.Resolve<TutorialProvider>();
                var asset = provider.Get(1);
                return id => new TutorialPresenter(Instantiate(asset, _noriceHolder));
            }, Lifetime.Singleton);
            Parent.Container.Resolve<TutorialProvider>();

            // パラメータ
            builder.Register<InGameParameter>(resolver =>
            {
                var container = resolver.Resolve<SceneParameterContainer>();
                InGameParameter parameter;
                if (container.Exists(SceneIndex.InGame))
                {
                    parameter = container.Fetch<InGameParameter>(SceneIndex.InGame);
                    // 削除しておく
                    container.Delete(SceneIndex.InGame);
                }
                else
                {
                    // Editor: InGameを直接再生した時のみ
                    parameter = _debugParameter;
                    Debug.Log($"Editor起動時の設定: {parameter}");
                }

                return parameter;
            }, Lifetime.Singleton);
        }
    }
}
