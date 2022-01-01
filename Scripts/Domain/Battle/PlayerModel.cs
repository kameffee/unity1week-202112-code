using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Deck;
using UnityEngine;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    public sealed class PlayerModel : IBattlePlayer
    {
        public PlayerStatusModel Status => _status;

        public FieldCardsModel FieldCards => _fieldCards;

        public IWaitCardSelect CardSelect => _waitCardSelect;

        private PlayerStatusModel _status;
        private FieldCardsModel _fieldCards;
        private PlayerCardDraw _cardDraw;
        private readonly IWaitCardSelect _waitCardSelect;

        private readonly int MaxCardCount = 6;

        public PlayerModel(
            PlayerStatusModel status,
            IWaitCardSelect waitCardSelect,
            PlayerCardDraw playerCardDraw)
        {
            _status = status;
            _cardDraw = playerCardDraw;
            _waitCardSelect = waitCardSelect;
            _fieldCards = new FieldCardsModel();
        }

        /// <summary>
        /// バトル前の準備
        /// </summary>
        public void BattlePrepare()
        {
            // HP前回
            _status.FullHp();
        }
        
        public UniTask TurnBegin()
        {
            _fieldCards.TurnBegin();
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// 最初のドロー
        /// </summary>
        public async UniTask InitialCardDraw(CancellationToken cancellation)
        {
            for (int i = 0; i < MaxCardCount; i++)
            {
                // 上段
                if (_cardDraw.Drawable())
                {
                    var cardForTop = _cardDraw.Draw();
                    _fieldCards.AddCard(cardForTop, 0);
                }

                // 下段
                if (_cardDraw.Drawable())
                {
                    var cardForBottom = _cardDraw.Draw();
                    _fieldCards.AddCard(cardForBottom, 1);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: cancellation);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellation);

            // 公開
            _fieldCards.UpdateCardsStatus();
        }

        /// <summary>
        /// カードを引く
        /// </summary>
        /// <param name="cancellation"></param>
        public async UniTask CardDraw(CancellationToken cancellation)
        {
            if (_fieldCards.TopCards.Count < MaxCardCount)
            {
                if (_cardDraw.Drawable())
                {
                    var card = _cardDraw.Draw();
                    _fieldCards.AddCard(card, 0);
                }
            }

            if (_fieldCards.BottomCards.Count < MaxCardCount)
            {
                if (_cardDraw.Drawable())
                {
                    var card = _cardDraw.Draw();
                    _fieldCards.AddCard(card, 1);
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellation);
        }

        public bool Selectable()
        {
            return _fieldCards.GetSelectableCards().Any();
        }

        public async UniTask<CommandCardModel> SelectCommand()
        {
            // 選ばれるまで待つ
            var selectedCard = await _waitCardSelect.WaitPlayerSelectCard();

            // 選択状態にする
            selectedCard.SetSelected(true);

            return selectedCard;
        }

        /// <summary>
        /// コマンドの使用
        /// </summary>
        /// <param name="useCard">使用するカード</param>
        /// <param name="otherPlayer"></param>
        public async UniTask UseCommand(CommandCardModel useCard, IBattlePlayer otherPlayer)
        {
            useCard.Use();

            // 実行
            switch (useCard.CommandEffect)
            {
                case IAttackEffect attackEffect:
                    await attackEffect.Execute(Status, otherPlayer.Status);
                    break;
                case IDamageTimingEffect damageTimingEffect:
                    _status.AddDamageTimingEffect(damageTimingEffect);
                    break;
                case IHpEffect healEffect:
                    healEffect.Heal(_status);
                    break;
                case IFieldCardsEffect fieldCardsEffect:
                    fieldCardsEffect.Execute(FieldCards, otherPlayer.FieldCards);
                    break;
                default:
                    Debug.LogError($"未定義: {useCard.CommandEffect.GetType()}");
                    break;
            }

            _fieldCards.UpdateCardsStatus();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        public bool IsEmptyHandsCard()
        {
            return _fieldCards.IsEmpty();
        }

        /// <summary>
        /// 全部なくなった際のリロード
        /// </summary>
        /// <returns></returns>
        public async UniTask ReInitializeDraw(CancellationToken cancellation)
        {
            _cardDraw.Reset();

            // セットアップ
            await InitialCardDraw(cancellation);
        }
    }
}
