using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Unity1week202112.Presentation.Status
{
    public interface IBuffStatusView
    {
        void SetIcon(Sprite sprite);

        UniTask PlayInAnimation();

        UniTask PlayOutAnimation();
    }
}
