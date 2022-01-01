using System;
using UniRx;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// 選択フェーズ操作
    /// </summary>
    public interface ICardSelectPhaseHandler
    {
        IObservable<Unit> OnStartPhase { get; }
        
        IObservable<Unit> OnEndPhase { get; }
        
        IReadOnlyReactiveProperty<bool> OnPhase { get; }
        
        void StartPhase();

        void EndPhase();
    }
}
