using System;
using UniRx;
using Unity1week202112.Presentation.UI;
using UnityEngine;

namespace Unity1week202112.Presentation.Title
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField]
        private CustomButton _startButton;

        [SerializeField]
        private CustomButton _settingsButton;

        public IObservable<Unit> OnClickStart => _startButton.OnClickAsObservable();
        
        public IObservable<Unit> OnClickSettings => _settingsButton.OnClickAsObservable();
    }
}
