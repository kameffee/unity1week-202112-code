using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202112.Presentation.Map
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldAntView : MonoBehaviour, IFieldPlayerView
    {
        public async UniTask Move(Vector3 worldPosition)
        {
            // 方向転換
            var isLeft = worldPosition.x < transform.position.x;
            transform.DOScaleX(Mathf.Abs(transform.localScale.x) * (isLeft ? 1 : -1), 0.2f);

            // 移動
            await transform.DOMove(worldPosition, 2f);
        }
    }
}
