using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using UnityEngine;

namespace Unity1week202112.Presentation.EffectDetail
{
    public class CommandEffectDetailModel : IDisposable
    {
        public string EffectName { get; }
        public string Description { get; }

        public IObservable<Sprite> Icon => _icon;
        private AsyncSubject<Sprite> _icon = new AsyncSubject<Sprite>();

        public IObservable<Unit> OnClose => _onClose;
        private readonly Subject<Unit> _onClose = new Subject<Unit>();

        private readonly int _effectType;
        private CommandEffectFactory _effectFactory;
        private readonly CommandEffectIconProvider _iconProvider;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CommandEffectDetailModel(int effectType,
            CommandEffectFactory effectFactory,
            CommandEffectIconProvider iconProvider)
        {
            _effectType = effectType;
            _effectFactory = effectFactory;
            _iconProvider = iconProvider;

            var data = _effectFactory.Get((int)effectType);
            EffectName = data.EffectName;
            Description = data.Description;
        }

        public void Initialize()
        {
            _iconProvider.GetIcon(_effectType).ToObservable()
                .Subscribe(sprite =>
                {
                    _icon.OnNext(sprite);
                    _icon.OnCompleted();
                })
                .AddTo(_disposable);
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        public void Close()
        {
            _onClose.OnNext(Unit.Default);
            _onClose.OnCompleted();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _onClose?.Dispose();
        }
    }
}
