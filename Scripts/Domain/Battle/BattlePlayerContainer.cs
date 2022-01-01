namespace Unity1week202112.Domain
{
    /// <summary>
    /// バトル参加者
    /// </summary>
    public sealed class BattlePlayerContainer
    {
        private IBattlePlayer _enemy;
        private IBattlePlayer _player;

        public IBattlePlayer GetPlayer() => _player;
        public IBattlePlayer GetEnemy() => _enemy;

        public void AddPlayer(IBattlePlayer player) => _player = player;
        public void AddEnemy(IBattlePlayer enemy) => _enemy = enemy;
    }
}
