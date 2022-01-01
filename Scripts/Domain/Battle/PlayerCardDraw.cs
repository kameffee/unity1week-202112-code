using Unity1week202112.Domain.Command;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// カードを引く
    /// </summary>
    public sealed class PlayerCardDraw
    {
        private readonly PlayerDeck _playerDeck;

        public PlayerCardDraw(PlayerDeck playerDeck)
        {
            _playerDeck = playerDeck;
        }

        /// <summary>
        /// カードが引けるか
        /// </summary>
        /// <returns></returns>
        public bool Drawable()
        {
            return _playerDeck.Drawable();
        }

        /// <summary>
        /// カードを引く
        /// </summary>
        /// <returns></returns>
        public CommandCardModel Draw()
        {
            return _playerDeck.Draw();
        }

        public void Reset()
        {
            _playerDeck.Reset();
        }
    }
}
