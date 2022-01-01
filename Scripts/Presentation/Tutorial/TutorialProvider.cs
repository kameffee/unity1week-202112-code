using UnityEngine;

namespace Unity1week202112.Presentation.Tutorial
{
    public class TutorialProvider
    {
        public TutorialView Get(int id)
        {
            return Resources.Load<TutorialView>($"Tutorials/Tutorial_{id:000}");
        }
    }
}
