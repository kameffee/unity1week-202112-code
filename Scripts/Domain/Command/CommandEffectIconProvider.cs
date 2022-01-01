using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Unity1week202112.Domain.Command
{
    public class CommandEffectIconProvider : IDisposable
    {
        private Dictionary<int, Sprite> _cache = new Dictionary<int, Sprite>();

        public async UniTask<Sprite> GetIcon(int typeId)
        {
            if (_cache.ContainsKey(typeId))
            {
                return _cache[typeId];
            }

            _cache[typeId] = await Resources.LoadAsync<Sprite>($"CardIcon/CardType_{typeId:000}") as Sprite;
            return _cache[typeId];
        }

        public void Dispose()
        {
            _cache = null;
        }
    }
}
