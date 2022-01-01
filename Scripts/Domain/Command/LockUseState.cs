using UnityEngine;

namespace Unity1week202112.Domain.Command
{
    public readonly struct LockUseState
    {
        /// <summary>
        /// 残り持続ターン数
        /// </summary>
        public int RemainingCount => _remainingCount;
        
        /// <summary>
        /// 付与ターンであるか
        /// </summary>
        public bool IsStartTurn => _isStartTurn;

        public bool Avairable => _remainingCount > 0;
        
        /// <summary>
        /// 効果が切れた
        /// </summary>
        public bool Finished => _remainingCount <= 0;

        private readonly int _remainingCount;
        private readonly bool _isStartTurn;

        public LockUseState(int remainingCount, bool isStartTurn = false)
        {
            _remainingCount = remainingCount;
            _isStartTurn = isStartTurn;
        }

        public static LockUseState Finish => new LockUseState(0);

        public static LockUseState operator --(LockUseState state)
        {
            return new LockUseState(Mathf.Max(state._remainingCount - 1, 0), false);
        }
    }
}
