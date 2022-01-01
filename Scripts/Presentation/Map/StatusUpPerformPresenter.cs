using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202112.Presentation.Map
{
    /// <summary>
    /// ステータスアップ演出
    /// </summary>
    public class StatusUpPerformPresenter : IInitializable, IStatusUpPerformPresenter
    {
        private readonly StatusUpWindow _view;

        public StatusUpPerformPresenter(StatusUpWindow view)
        {
            _view = view;
        }

        public void Initialize()
        {
        }

        /// <summary>
        /// ステータスアップの演出を行う
        /// </summary>
        /// <param name="isWin"></param>
        /// <param name="statusChange"></param>
        /// <param name="cancellationToken"></param>
        public async UniTask Perform(bool isWin, StatusChange statusChange, CancellationToken cancellationToken)
        {
            _view.Initialize(isWin);
            await _view.Open(statusChange, cancellationToken);

            await UniTask.WaitUntil(() => Input.anyKeyDown || Input.GetMouseButtonDown(0),
                cancellationToken: cancellationToken);

            await _view.Close(cancellationToken);
        }
    }
}
