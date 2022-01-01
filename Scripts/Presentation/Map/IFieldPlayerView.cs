using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Unity1week202112.Presentation.Map
{
    public interface IFieldPlayerView
    {
        UniTask Move(Vector3 worldPosition);
    }
}