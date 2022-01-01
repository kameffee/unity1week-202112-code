using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using VContainer.Unity;

namespace Unity1week202112.Presentation.CommandCard
{
    /// <summary>
    /// 手札
    /// </summary>
    public class UserHandsPresenter : IStartable
    {
        private readonly FieldCardsModel _model;
        private readonly Func<ICommandCardView> _cardViewFactory;
        private readonly Func<CommandCardModel, ICommandCardView, ICommandCardPresenter> _presenterFactory;
        private readonly CardSlotList _cardSlotList;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public UserHandsPresenter(IBattlePlayer model,
            Func<ICommandCardView> viewFactory,
            Func<CommandCardModel, ICommandCardView, ICommandCardPresenter> presenterFactory,
            CardSlotList cardSlotList)
        {
            _model = model.FieldCards;
            _cardViewFactory = viewFactory;
            _presenterFactory = presenterFactory;
            _cardSlotList = cardSlotList;
        }

        public void Start()
        {
            // 追加された時
            _model.OnAdd
                .Subscribe(card => AddCard(card))
                .AddTo(_disposable);

            // 使用された
            _model.OnUse
                .Subscribe(model =>
                {
                    _model.RemoveCard(model);
                })
                .AddTo(_disposable);
        }

        /// <summary>
        /// カード追加
        /// </summary>
        /// <param name="model"></param>
        private void AddCard(CommandCardModel model)
        {
            var cardView = _cardViewFactory.Invoke();
            var cardPresenter = _presenterFactory.Invoke(model, cardView);
            cardPresenter.Initialize();

            var holder = _cardSlotList.GetHolder(model.Position.Value);
            cardView.SetPosition(holder.anchoredPosition);

            cardPresenter.PlayInAnimation().Forget();
        }
    }
}
