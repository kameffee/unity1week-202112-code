using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Deck;
using Unity1week202112.Domain.Map;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Presentation;
using Unity1week202112.Presentation.CommandCard;
using Unity1week202112.Presentation.Status;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Installer
{
    public class EnemyLifetimeScope : LifetimeScope
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
            var parameter = Parent.Container.Resolve<InGameParameter>();
            builder.RegisterComponentInNewPrefab(charaProvider.Load<CharacterView>(parameter.EnemyCharacterId),
                    Lifetime.Singleton)
                .UnderTransform(_worldRoot)
                .As<IDamageFeedbackView>()
                .AsSelf();

            builder.Register<PlayerStatusModel>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .WithParameter(parameter.EnemyHp)
                .AsSelf();
            builder.Register<EnemyDeckRepository>(Lifetime.Singleton)
                .WithParameter(Parent.Container.Resolve<InGameParameter>().EnemyDeckGroupId)
                .AsImplementedInterfaces();
            builder.Register<PlayerDeck>(Lifetime.Singleton);
            builder.Register<PlayerCardDraw>(Lifetime.Singleton);
            builder.Register<EnemyModel>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            // プレイヤーカード
            builder.RegisterComponentInHierarchy<CardSlotList>()
                .UnderTransform(_uiRoot)
                .AsSelf();

            builder.RegisterFactory<ICommandCardView>(
                resolver => { return () => Instantiate(_cardPrefab, _cardHolder); },
                Lifetime.Scoped);

            builder.RegisterFactory<CommandCardModel, ICommandCardView, ICommandCardPresenter>(resolver =>
            {
                // 敵のカード用のPresenterを分ける
                var enemy = resolver.Resolve<EnemyModel>();
                return (model, view) => new OtherPlayerCommandCardPresenter(model, view,
                    resolver.Resolve<IWaitCardSelect>(),
                    resolver.Resolve<CommandEffectIconProvider>(),
                    enemy.FieldCards,
                    resolver.Resolve<CardSlotList>());
            }, Lifetime.Scoped);

            builder.RegisterEntryPoint<UserHandsPresenter>(Lifetime.Scoped);

            // プレイヤーステータス
            builder.RegisterComponent<PlayerHpView>(_hpView);
            builder.RegisterComponent<BuffView>(_buffView).As<IBuffStatusView>();
            builder.RegisterEntryPoint<EnemyStatusPresenter>();

            builder.RegisterBuildCallback(resolver =>
            {
                var enemy = resolver.Resolve<EnemyModel>();
                resolver.Resolve<BattlePlayerContainer>().AddEnemy(enemy);
            });
        }
    }
}
