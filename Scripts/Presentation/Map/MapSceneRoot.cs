using Unity1week202112.Domain;
using UnityEngine;

namespace Unity1week202112.Presentation.Map
{
    public class MapSceneRoot : MonoBehaviour, ISceneRoot
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
