using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.Map;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Domain.User;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// バトルシーンへの遷移
    /// </summary>
    public class TransitionToBattle
    {
        [Inject]
        private readonly IScreenFader _fader;

        [Inject]
        private readonly SceneParameterContainer _sceneParameterContainer;

        private readonly ISceneRoot _sceneRoot;
        private readonly MapDataFactory _mapDataFactory;
        private readonly UserRepository _userRepository;

        public TransitionToBattle(
            ISceneRoot sceneRoot,
            MapDataFactory mapDataFactory,
            UserRepository userRepository)
        {
            this._sceneRoot = sceneRoot;
            _mapDataFactory = mapDataFactory;
            _userRepository = userRepository;
        }

        public async UniTask StartBattle(MapPoint mapPoint, CancellationToken cancellation)
        {
            await _fader.FadeOut(1f, cancellation);

            // 敵をセット
            var data = _mapDataFactory.Get(mapPoint.PointId);
            var userData = _userRepository.Load();
            var param = new InGameParameter(
                data.EnemyHp,
                data.EnemyDeckGroupId,
                data.EnemyDeckGroupId,
                !userData.ShownTutorial);
            _sceneParameterContainer.Push(SceneIndex.InGame, param);

            // 保存
            userData.ShownTutorial = true;
            _userRepository.Save(userData);

            // 遷移
            await SceneManager.LoadSceneAsync((int)SceneIndex.InGame, LoadSceneMode.Additive);
            _sceneRoot.SetActive(false);
            var loadedScene = SceneManager.GetSceneByBuildIndex((int)SceneIndex.InGame);
            SceneManager.SetActiveScene(loadedScene);

            // 終わるまで待つ
            bool isUnload = false;
            SceneManager.sceneUnloaded += scene =>
            {
                if (scene.buildIndex == (int)SceneIndex.InGame)
                {
                    isUnload = true;
                }
            };

            await UniTask.WaitUntil(() => isUnload, cancellationToken: cancellation);

            _sceneRoot.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);
            await _fader.FadeIn(cancellationToken: cancellation);
        }
    }
}
