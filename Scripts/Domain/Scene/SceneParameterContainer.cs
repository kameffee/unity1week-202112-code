using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202112.Domain.Scene
{
    public interface ISceneParameter
    {
    }

    /// <summary>
    /// シーン間のデータの橋渡し
    /// </summary>
    public class SceneParameterContainer
    {
        private Dictionary<int, ISceneParameter> _parameterDic = new Dictionary<int, ISceneParameter>();

        public void Push(int sceneIndex, ISceneParameter sceneParameter)
        {
            _parameterDic[sceneIndex] = sceneParameter;
        }

        public bool Exists(int sceneIndex)
        {
            return _parameterDic.ContainsKey(sceneIndex);
        }

        public T Fetch<T>(int sceneIndex) where T : ISceneParameter
        {
            if (_parameterDic.TryGetValue(sceneIndex, out var param))
            {
                return (T)param;
            }

            throw new ArgumentException($"Index:{sceneIndex}");
        }

        public void Delete(int sceneIndex)
        {
            if (_parameterDic.ContainsKey(sceneIndex))
            {
                _parameterDic.Remove(sceneIndex);
                return;
            }

            Debug.LogWarning($"削除するパラメータがありません. id:{sceneIndex}");
        }
    }

    public static class SceneParameterEx
    {
        public static void Push(this SceneParameterContainer container, SceneIndex sceneIndex, ISceneParameter sceneParameter)
        {
            container.Push((int)sceneIndex, sceneParameter);
        }

        public static bool Exists(this SceneParameterContainer container, SceneIndex sceneIndex)
        {
            return container.Exists((int)sceneIndex);
        }

        public static T Fetch<T>(this SceneParameterContainer container, SceneIndex sceneIndex) where T : ISceneParameter
        {
            return container.Fetch<T>((int)sceneIndex);
        }

        public static void Delete(this SceneParameterContainer container, SceneIndex sceneIndex)
        {
            container.Delete((int)sceneIndex);
        }
    }
}
