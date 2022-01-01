using System;
using UniRx;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// バトルのフェーズ状態
    /// </summary>
    public sealed class BattleStatus : ICardSelectPhaseHandler
    {
        // フェイズ開始
        public IObservable<Unit> OnStartPhase => _onStartPhase;
        private readonly Subject<Unit> _onStartPhase = new Subject<Unit>();

        // フェイズ終了
        public IObservable<Unit> OnEndPhase => _onEndPhase;
        private readonly Subject<Unit> _onEndPhase = new Subject<Unit>();

        // フェイズ中か
        public IReadOnlyReactiveProperty<bool> OnPhase => _onPhase;
        private readonly ReactiveProperty<bool> _onPhase = new BoolReactiveProperty();

        /// <summary>
        /// 開始
        /// </summary>
        public void StartPhase()
        {
            _onPhase.Value = true;
            _onStartPhase.OnNext(Unit.Default);
        }

        public void EndPhase()
        {
            _onPhase.Value = false;
            _onEndPhase.OnNext(Unit.Default);
        }
    }
}
