using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity1week202112.Presentation.UI
{
    public class TextShake : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _messageText;

        private readonly List<Tween> _tweenList = new List<Tween>();

        private void Start()
        {
            Shake();
        }

        public void Shake()
        {
            _tweenList.ForEach(tween => tween?.Kill());

            DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(_messageText);

            // 
            for (int i = 0; i < tmproAnimator.textInfo.characterCount; ++i)
            {
                // 透明化
                _tweenList.Add(tmproAnimator.DOFadeChar(i, 0f, 0));
                // 登場開始アニメ
                _tweenList.Add(DOTween.Sequence()
                    .Append(tmproAnimator.DOFadeChar(i, 1, 0.5f).SetEase(Ease.OutSine))
                    .SetDelay(Random.Range(0.1f, 1.0f)).Play());

                // 揺らす
                _tweenList.Add(tmproAnimator.DOShakeCharOffset(i, 1f, 1, 2)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear));
            }
        }
        
        public void FadeOut()
        {
            _tweenList.ForEach(tween => tween?.Kill());

            DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(_messageText);

            // 
            for (int i = 0; i < tmproAnimator.textInfo.characterCount; ++i)
            {
                // 登場開始アニメ
                _tweenList.Add(DOTween.Sequence()
                    .Append(tmproAnimator.DOFadeChar(i, 0, 1f).SetEase(Ease.OutSine))
                    .SetDelay(Random.Range(0.1f, 1.0f)).Play());

                // 揺らす
                _tweenList.Add(tmproAnimator.DOShakeCharOffset(i, 1f, 1, 2)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear));
            }
        }
    }
}
