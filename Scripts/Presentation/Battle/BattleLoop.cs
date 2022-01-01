using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Presentation.Tutorial;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity1week202112.Presentation
{
    /// <summary>
    /// バトルパートのループ
    /// </summary>
    public sealed class BattleLoop
    {
        private readonly ICardSelectPhaseHandler _cardSelectPhaseHandler;
        private readonly GameTurn _gameTurn;

        // 演出系
        private readonly IBattleStartPerformer _battleStartPerformer;
        private readonly IBattleWinPerformer _battleWinPerformer;
        private readonly IBattleLosePerformer _battleLosePerformer;

        // チュートリアル
        private Func<int, TutorialPresenter> _tutorialFactory;
        private readonly InGameParameter _inGameParameter;

        // 自分
        private IBattlePlayer _player;

        // 相手
        private IBattlePlayer _enemy;

        public BattleLoop(
            ICardSelectPhaseHandler cardSelectPhaseHandler,
            GameTurn gameTurn,
            IBattleStartPerformer battleStartPerformer,
            IBattleWinPerformer battleWinPerformer,
            IBattleLosePerformer battleLosePerformer,
            Func<int, TutorialPresenter> tutorialFactory,
            InGameParameter inGameParameter)
        {
            _cardSelectPhaseHandler = cardSelectPhaseHandler;
            _gameTurn = gameTurn;
            _battleStartPerformer = battleStartPerformer;
            _battleWinPerformer = battleWinPerformer;
            _battleLosePerformer = battleLosePerformer;
            _tutorialFactory = tutorialFactory;
            _inGameParameter = inGameParameter;
        }

        public void Initialize(IBattlePlayer player, IBattlePlayer enemy)
        {
            _player = player;
            _enemy = enemy;
        }

        public async UniTask<BattleCondition> StartAsync(CancellationToken cancellation)
        {
            Assert.IsNotNull(_enemy);
            Assert.IsNotNull(_enemy);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);

            var battleCondition = await BattleSequence(cancellation);

            return battleCondition;
        }

        public async UniTask<BattleCondition> BattleSequence(CancellationToken cancellation)
        {
            // 準備
            _player.BattlePrepare();
            _enemy.BattlePrepare();

            // 開始演出
            await _battleStartPerformer.Perform(cancellation);

            // 最初に数枚引く
            var enemyDraw = _enemy.InitialCardDraw(cancellation);
            var playerDraw = _player.InitialCardDraw(cancellation);

            await (enemyDraw, playerDraw);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);


            if (_inGameParameter.IsTutorial)
            {
                // チュートリアル演出
                var tutorialPresenter = _tutorialFactory.Invoke(1);
                await tutorialPresenter.StartAsync(cancellation);
            }

            while (!cancellation.IsCancellationRequested)
            {
#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    // 強制勝利
                    Debug.Log("強制勝利");
                    return BattleCondition.Win;
                }
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    // 強制勝利
                    Debug.Log("強制敗北");
                    return BattleCondition.Lose;
                }
#endif

                // 次のターンへ
                _gameTurn.NextTurn();
                Debug.Log($"{_gameTurn.TurnCount.Value}ターン目 開始");

                UniTask playerReDraw = _player.IsEmptyHandsCard()
                    ? _player.ReInitializeDraw(cancellation)
                    : UniTask.CompletedTask;

                UniTask enemyReDraw = _enemy.IsEmptyHandsCard()
                    ? _enemy.ReInitializeDraw(cancellation)
                    : UniTask.CompletedTask;

                await UniTask.WhenAll(playerReDraw, enemyReDraw);

                // ターン開始
                await UniTask.WhenAll(_player.TurnBegin(), _enemy.TurnBegin());

                // Todo: ターン開始演出
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: cancellation);

                // カード選択フェーズ開始
                _cardSelectPhaseHandler.StartPhase();

                // 入力待ち
                // プレイヤーの行動を決める

                CommandCardModel playerSelectCommand = null;
                if (_player.Selectable())
                {
                    playerSelectCommand = await _player.SelectCommand();
                    Debug.Log($"PlayerCard: {playerSelectCommand.CommandName}");
                }
                else
                {
                    Debug.Log("Player スキップ");
                }

                // カード選択フェーズ終了
                _cardSelectPhaseHandler.EndPhase();

                await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: cancellation);

                CommandCardModel enemySelectCommand = null;
                // 選択できるカードがあるか
                if (_enemy.Selectable())
                {
                    // 敵の行動を選択
                    enemySelectCommand = await _enemy.SelectCommand();
                    Debug.Log($"EnemyCard: {enemySelectCommand.CommandName}");
                }
                else
                {
                    Debug.Log("Enemy スキップ");
                }

                Debug.Log($"行動演出中...");
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellation);


                // プレイヤーのコマンドカード使用
                if (playerSelectCommand != null)
                {
                    await _player.UseCommand(playerSelectCommand, _enemy);
                }

                // 状態チェック
                var enemyAfterCondition = CheckBattleCondition();
                if (enemyAfterCondition != BattleCondition.Continue)
                {
                    break;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: cancellation);

                // 敵ターン
                if (enemySelectCommand != null)
                {
                    await _enemy.UseCommand(enemySelectCommand, _player);
                }

                // 状態チェック
                var playerAfterCondition = CheckBattleCondition();
                if (playerAfterCondition != BattleCondition.Continue)
                {
                    break;
                }

                // コマンドカードを引く
                await _enemy.CardDraw(cancellation);
                await _player.CardDraw(cancellation);

                Debug.Log($"{_gameTurn.TurnCount.Value}ターン目 終了");
            }

            var condition = CheckBattleCondition();
            switch (condition)
            {
                case BattleCondition.Win:
                    await _battleWinPerformer.Perform(cancellation);
                    break;
                case BattleCondition.Lose:
                    await _battleLosePerformer.Perform(cancellation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log("バトル終了");

            return condition;
        }

        private BattleCondition CheckBattleCondition()
        {
            if (_enemy.Status.IsDead())
            {
                return BattleCondition.Win;
            }

            if (_player.Status.IsDead())
            {
                return BattleCondition.Lose;
            }

            return BattleCondition.Continue;
        }
    }
}
