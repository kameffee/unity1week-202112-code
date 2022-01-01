using System;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.Command;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// カード選択
    /// </summary>
    public interface IWaitCardSelect
    {
        IObservable<CommandCardModel> OnSelectCard { get; }

        UniTask<CommandCardModel> WaitPlayerSelectCard();

        void Select(CommandCardModel cardModel);
    }
}
