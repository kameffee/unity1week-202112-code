using System;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Deck;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Domain.User;
using Unity1week202112.Presentation;
using Unity1week202112.Presentation.CommandCard;
using Unity1week202112.Presentation.EffectDetail;
using Unity1week202112.Presentation.Status;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Installer
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private Transform _uiRoot;

        [SerializeField]
        private Transform _worldRoot;

        [SerializeField]
        private BuffView _buffView;

        [SerializeField]
        private PlayerHpView _hpView;

        [SerializeField]
        private CommandCardView _cardPrefab;

        [SerializeField]
        private Transform _cardHolder;

        protected override void Configure(IContainerBuilder builder)
        {
            // キャラ生成
            var charaProvider = Parent.Container.Resolve<BattleCharacterPrefabProvider>();
            builder.RegisterComponentInNewPrefab(charaProvider.Load<CharacterView>(0), Lifetime.Singleton)
                .UnderTransform(_worldRoot)
                .As<IDamageFeedbackView>()
                .AsSelf();

            var userData = Parent.Container.Resolve<UserRepository>().Load();
            
            builder.Register<PlayerStatusModel>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .WithParameter(userData.Hp)
                .AsSelf();

            builder.Register<PlayerDeck>(Lifetime.Singleton);
            builder.RegisterInstance(Parent.Container.Resolve<PlayerDeckRepository>())
                .As<IDeckRepository>();
            builder.Register<PlayerCardDraw>(Lifetime.Singleton);
            builder.Register<PlayerModel>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterEntryPoint<BattleCharacterFeedbackPresenter>();

            // プレイヤーカード
            builder.RegisterComponentInHierarchy<CardSlotList>()
                .UnderTransform(_uiRoot)
                .AsSelf();

            builder.RegisterFactory<ICommandCardView>(
                resolver => () => Instantiate(_cardPrefab, _cardHolder),
                Lifetime.Scoped);

            builder.RegisterFactory<CommandCardModel, ICommandCardView, ICommandCardPresenter>(resolver =>
            {
                var player = resolver.Resolve<PlayerModel>();
                return (model, view) => new CommandCardPresenter(model, view,
                    player.CardSelect,
                    resolver.Resolve<CommandEffectIconProvider>(),
                    resolver.Resolve<ICardSelectPhaseHandler>(),
                    player.FieldCards,
                    resolver.Resolve<CardSlotList>(),
                    resolver.Resolve<Func<int, CommandEffectDetailModel>>());
            }, Lifetime.Scoped);

            builder.RegisterEntryPoint<UserHandsPresenter>(Lifetime.Scoped);

            // プレイヤーステータス
            builder.RegisterComponent<PlayerHpView>(_hpView);
            builder.RegisterComponent<BuffView>(_buffView).As<IBuffStatusView>();
            builder.RegisterEntryPoint<PlayerStatusPresenter>();

            builder.RegisterBuildCallback(resolver =>
            {
                var player = resolver.Resolve<PlayerModel>();
                resolver.Resolve<BattlePlayerContainer>().AddPlayer(player);
            });
        }
    }
}
