using System.Collections.Generic;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Lottery;
using Unity1week202112.Domain.Map;
using Unity1week202112.Domain.Message;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Domain.TradeCard;
using Unity1week202112.Presentation;
using Unity1week202112.Presentation.CommandCard;
using Unity1week202112.Presentation.Map;
using Unity1week202112.Presentation.Talk;
using Unity1week202112.Presentation.Trade;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Installer
{
    public class MapLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private List<MapFieldBattlePoint> _battlePoints;

        [SerializeField]
        private AreaTitle _areaTitlePrefab;

        [SerializeField]
        private Transform _holder;

        [SerializeField]
        private MessageWindow _godMessageWindowPrefab;

        [SerializeField]
        private StatusUpWindow _statusUpWindowPrefab;

        [Header("Trade")]
        [SerializeField]
        private CardTradeWindow _cardTradeViewPrefab;

        [SerializeField]
        private CommandCardView _commandCardViewPrefab;

        [Header("Debug")]
        [SerializeField]
        private MapSceneParameter _debugSceneParameter;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MapDataFactory>(Lifetime.Singleton);

            builder.RegisterInstance(new MapFieldBattlePointList(_battlePoints));
            builder.RegisterComponentInHierarchy<FieldAntView>()
                .As<IFieldPlayerView>()
                .AsSelf();
            builder.RegisterEntryPoint<FieldPlayerPresenter>();
            builder.Register<MapWork>(Lifetime.Singleton);
            builder.Register<TransitionToBattle>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<MapSceneRoot>()
                .As<ISceneRoot>()
                .AsSelf();

            builder.RegisterComponentInNewPrefab<AreaTitle>(_areaTitlePrefab, Lifetime.Singleton)
                .UnderTransform(_holder)
                .As<IAreaTitlePerformer>()
                .AsSelf();

            var container = Parent.Container.Resolve<SceneParameterContainer>();
            if (container.Exists(SceneIndex.Title))
            {
                var parameter = container.Fetch<MapSceneParameter>(SceneIndex.Title);
                builder.RegisterInstance<MapSceneParameter>(parameter);
                // 取得したら消しておく
                container.Delete(SceneIndex.Title);
            }
            else
            {
                builder.RegisterInstance(_debugSceneParameter);
                Debug.Log($"Editorの直接起動: startPoint:{_debugSceneParameter.StartPoint}");
            }

            // メッセージ
            builder.RegisterComponentInNewPrefab<MessageWindow>(_godMessageWindowPrefab, Lifetime.Singleton)
                .UnderTransform(_holder)
                .As<IMessageWindow>()
                .AsSelf();

            builder.Register<MessageFactory>(Lifetime.Scoped);
            builder.Register<MessageProvider>(Lifetime.Scoped);
            builder.Register<TalkMessageScenario>(Lifetime.Scoped);
            builder.RegisterEntryPoint<MessagePresenter>();

            // ステータスUP
            builder.RegisterComponentInNewPrefab<StatusUpWindow>(_statusUpWindowPrefab, Lifetime.Scoped)
                .UnderTransform(_holder)
                .AsSelf();
            builder.Register<StatusUpPerformPresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<StatusUpEvent>(Lifetime.Scoped);

            // カード交換
            builder.Register<TradeCardEvent>(Lifetime.Scoped);
            builder.Register<LotteryCard>(Lifetime.Scoped);
            builder.Register<CommandEffectIconProvider>(Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab<CardTradeWindow>(_cardTradeViewPrefab, Lifetime.Scoped)
                .UnderTransform(_holder);
            builder.Register<CardTradePresenter>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterFactory<Transform, TradeSelectCardModel, TradeSelectCardPresenter>(resolver =>
            {
                return (holder, model) =>
                {
                    var view = Instantiate(_commandCardViewPrefab, holder);
                    return new TradeSelectCardPresenter(model,
                        view,
                        resolver.Resolve<CommandEffectIconProvider>(),
                        null);
                };
            }, Lifetime.Scoped);

            builder.Register<EndingLoop>(Lifetime.Scoped);

            builder.RegisterEntryPoint<MapLoop>();
        }
    }
}
