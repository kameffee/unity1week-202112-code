using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Message;
using Unity1week202112.Domain.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Presentation
{
    /// <summary>
    /// インゲームのメインループ
    /// </summary>
    public sealed class GameLoop : IAsyncStartable
    {
        [Inject]
        private readonly IScreenFader _fader;

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private IBattlePlayer _playerModel;
        private IBattlePlayer _enemyModel;

        private readonly SceneParameterContainer _sceneParameterContainer;
        private readonly BattleLoop _battleLoop;
        private readonly BattlePlayerContainer _playerContainer;
        private readonly TalkMessageScenario _talkMessageScenario;
        private readonly InGameParameter _inGameParameter;

        public GameLoop(
            SceneParameterContainer sceneParameterContainer,
            BattleLoop battleLoop,
            BattlePlayerContainer playerContainer,
            TalkMessageScenario talkMessageScenario,
            InGameParameter inGameParameter)
        {
            _sceneParameterContainer = sceneParameterContainer;
            _battleLoop = battleLoop;
            _playerContainer = playerContainer;
            _talkMessageScenario = talkMessageScenario;
            _inGameParameter = inGameParameter;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _audioPlayer.Bgm.CrossFade(1, 1f);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);
            await _fader.FadeIn(1f, cancellation);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: cancellation);

            // 仮: 会話パート
            int charaId = _inGameParameter.EnemyCharacterId;
            await _talkMessageScenario.StartTalkScenario(charaId * 100, cancellation);

            // Todo: 敵の決定

            // プレイヤーと対戦相手
            _playerModel = _playerContainer.GetPlayer();
            _enemyModel = _playerContainer.GetEnemy();
            _battleLoop.Initialize(_playerModel, _enemyModel);

            // バトル
            BattleCondition battleCondition = await _battleLoop.StartAsync(cancellation);

            if (battleCondition == BattleCondition.Win)
            {
                // 敵が負けた時の会話
                await _talkMessageScenario.StartTalkScenario(charaId * 100 + 1, cancellation);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellation);

            await _fader.FadeOut(1f, cancellation);

            // Todo: 結果を保存して戻る
            _sceneParameterContainer.Push(SceneIndex.InGame, new BattleResultParameter(battleCondition));
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
    }
}
