using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.Command;

namespace Unity1week202112.Domain.TradeCard
{
    public interface ITradeCardPresenter
    {
        UniTask<CommandCardModel> StartSelectCardPhase(IEnumerable<CommandCardModel> gettableCards, CancellationToken cancellation);

        UniTask<CommandCardModel> StartReleaseSelectCardPhase(IEnumerable<CommandCardModel> releaseCards, CancellationToken cancellation);
    }
}
