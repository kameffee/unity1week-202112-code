using UniRx;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// ターンを管理
    /// </summary>
    public sealed class GameTurn
    {
        // 現在のターン数
        public IReadOnlyReactiveProperty<int> TurnCount => _turnCount;
        private readonly ReactiveProperty<int> _turnCount = new IntReactiveProperty();

        public GameTurn()
        {
            // 0ターン開始
            _turnCount.Value = 0;
        }

        /// <summary>
        /// 次のターンへカウントアップする
        /// </summary>
        public void NextTurn()
        {
            _turnCount.Value += 1;
        }
    }
}
