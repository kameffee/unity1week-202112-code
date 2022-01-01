using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Unity1week202112.Presentation.CommandCard
{
    public interface ICommandCardView
    {
        /// <summary>
        /// クリックされた時
        /// </summary>
        IObservable<Unit> OnClick { get; }

        IObservable<Unit> OnLongPress { get; }
        IObservable<Unit> OnRightClick { get; }

        void SetImage(Sprite sprite);

        void SetNum(int num);

        void SetPosition(Vector2 pos);

        /// <summary>
        /// 引いた時のアニメーション
        /// </summary>
        UniTask PlayInAnimation();

        /// <summary>
        /// 使用時アニメーション
        /// </summary>
        UniTask PlayUseAnimation(CancellationToken cancellation);

        UniTask Move(Vector2 toPos);

        UniTask Show();

        void Delete();

        void SetSelectedState(bool isSelected, bool forceLastSibling = true);

        void SetSelectableState(bool selectable);

        void SetEnableFeedBack(bool isEnable);

        void SetLockCount(int count);
    }
}
