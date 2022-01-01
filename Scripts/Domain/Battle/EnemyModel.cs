using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Deck;
using Unity1week202112.Domain.Extensions;
using Unity1week202112.Domain.Scene;
using UnityEngine;

namespace Unity1week202112.Domain
{
    public sealed class EnemyModel : IBattlePlayer
    {
        public PlayerStatusModel Status => _status;
        public FieldCardsModel FieldCards => _fieldCards;

        private readonly PlayerStatusModel _status;
        private readonly FieldCardsModel _fieldCards;
        private readonly PlayerCardDraw _cardDraw;

        private readonly int MaxCardCount = 6;

        public EnemyModel(PlayerStatusModel status, PlayerCardDraw playerCardDraw, InGameParameter parameter)
        {
            var deckId = parameter.EnemyDeckGroupId;
            Debug.Log($"Enemy DeckId:{deckId}");
            _cardDraw = playerCardDraw;
            _status = status;
            _fieldCards = new FieldCardsModel();
        }

        public void BattlePrepare()
        {
            _status.FullHp();
        }

        public UniTask TurnBegin()
        {
            _fieldCards.TurnBegin();
            return UniTask.CompletedTask;
        }

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

        public UniTask CardDraw(CancellationToken cancellation)
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

            return UniTask.CompletedTask;
        }

        public bool Selectable()
        {
            return _fieldCards.GetSelectableCards().Any();
        }

        public UniTask<CommandCardModel> SelectCommand()
        {
            // 先頭2枚を適当に一枚選ぶ
            var card = _fieldCards.GetSelectableCards().Shuffle().First();
            card.SetSelected(true);

            return new UniTask<CommandCardModel>(card);
        }

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
