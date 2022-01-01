using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202112.Presentation
{
    public class TurnView : MonoBehaviour, ITurnView
    {
        [SerializeField]
        private TextMeshProUGUI _turn;

        public void RenderTurn(int turnCount)
        {
            _turn.SetText(turnCount.ToString());
            var rect = transform as RectTransform;
            rect.DOShakeAnchorPos(0.5f, 10f, 20);
        }
    }
}
