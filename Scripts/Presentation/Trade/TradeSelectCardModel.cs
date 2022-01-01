using System;
using UniRx;
using Unity1week202112.Domain.Command;

namespace Unity1week202112.Presentation.Trade
{
    public sealed class TradeSelectCardModel
    {
        public IObservable<Unit> OnSelect => _onSelect;
        private readonly Subject<Unit> _onSelect = new Subject<Unit>();

        public CommandCardModel CommandCardModel { get; }

        public TradeSelectCardModel(CommandCardModel commandCardModel)
        {
            CommandCardModel = commandCardModel;
        }

        public void Select()
        {
            _onSelect.OnNext(Unit.Default);
        }
    }
}