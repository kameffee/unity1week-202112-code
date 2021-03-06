using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Map;
using Unity1week202112.Domain.Scene;
using Unity1week202112.Domain.TradeCard;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Presentation.Map
{
    public sealed class MapLoop : IInitializable, IAsyncStartable
    {
        [Inject]
        private IScreenFader _fader;

        [Inject]
        private SceneParameterContainer _sceneParameterContainer;

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private readonly EndingLoop _endingLoop;
        private readonly StatusUpEvent _statusUpEvent;
        private readonly TradeCardEvent _tradeCardEvent;

        [Inject]
        private IAreaTitlePerformer _areaTitlePerformer;

        private MapWork _mapWork;
        private readonly MapDataFactory _mapDataFactory;
        private readonly TransitionToBattle _transitionToBattle;
        private readonly MapSceneParameter _mapSceneParameter;

        public MapLoop(MapWork mapWork,
            MapDataFactory mapDataFactory,
            TransitionToBattle transitionToBattle,
            MapSceneParameter mapSceneParameter,
            SceneParameterContainer sceneParameterContainer,
            EndingLoop endingLoop,
            StatusUpEvent statusUpEvent,
            TradeCardEvent tradeCardEvent)
        {
            _mapWork = mapWork;
            _mapDataFactory = mapDataFactory;
            _transitionToBattle = transitionToBattle;
            _mapSceneParameter = mapSceneParameter;
            _sceneParameterContainer = sceneParameterContainer;
            _endingLoop = endingLoop;
            _statusUpEvent = statusUpEvent;
            _tradeCardEvent = tradeCardEvent;
        }

        public void Initialize()
        {
            _mapWork.Initialize(_mapSceneParameter.StartPoint);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _fader.FadeIn(cancellationToken: cancellation);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);

            MapPoint mapPoint = _mapSceneParameter.StartPoint;

            // ???????????????????????????????????????
            while (!_mapWork.IsEnd())
            {
                await _mapWork.Move(mapPoint);

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);

                // ??????????????????????????? ??????
                var mapData = _mapDataFactory.Get(mapPoint.PointId);
                await _areaTitlePerformer.Perform(mapData.AreaTitle, cancellation);

                // ?????????????????????????????????
                var delay = UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: cancellation);
                var input = UniTask.WaitUntil(() => Input.anyKeyDown || Input.GetMouseButtonDown(0),
                    cancellationToken: cancellation);
                await UniTask.WhenAny(delay, input);

                // ??????????????????????????? ??????
                await _areaTitlePerformer.Close(cancellation);

                // ???????????????????????????
                await _transitionToBattle.StartBattle(mapPoint, cancellation);

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);

                // ?????????????????????.
                var result = _sceneParameterContainer.Fetch<BattleResultParameter>(SceneIndex.InGame);
                if (result.BattleCondition == BattleCondition.Win)
                {
                    if (mapPoint.PointId + 1 > 6)
                    {
                        // ??????
                        break;
                    }
                    
                    // ????????????????????????
                    await _statusUpEvent.Execute(1000, cancellation);

                    // ???????????????????????????
                    await _tradeCardEvent.Execute(mapPoint, cancellation);

                    mapPoint++;
                }
                else
                {
                    // ?????????????????????
                    mapPoint = new MapPoint(0);

                    // ????????????????????????
                    await _statusUpEvent.Execute(2000, cancellation);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellation);
            }

            await EndingSequence(cancellation);
        }

        private async UniTask EndingSequence(CancellationToken cancellation)
        {
            // ????????????????????????
            await _endingLoop.Start(cancellation);

            _audioPlayer.Bgm.Stop(2);
            await _fader.FadeOut(2f, cancellation);

            // ?????????????????????
            await SceneManager.LoadSceneAsync((int)SceneIndex.Title);
        }
    }
}
