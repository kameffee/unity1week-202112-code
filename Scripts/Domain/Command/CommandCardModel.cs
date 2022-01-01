using System;
using UniRx;
using UnityEngine;

namespace Unity1week202112.Domain.Command
{
    /// <summary>
    /// コマンドカード
    /// </summary>
    public class CommandCardModel : IEquatable<CommandCardModel>
    {
        public int Id { get; }

        /// <summary>
        /// 名前
        /// </summary>
        public string CommandName => _commandName;

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 効果
        /// </summary>
        public ICommandEffect CommandEffect { get; }

        public int EffectNum { get; }

        public IReadOnlyReactiveProperty<bool> Visible => _visible;
        private readonly ReactiveProperty<bool> _visible = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<Vector2Int> Position => _position;
        private readonly ReactiveProperty<Vector2Int> _position = new ReactiveProperty<Vector2Int>();

        public IObservable<Unit> OnUse => _onUse;
        private readonly Subject<Unit> _onUse = new Subject<Unit>();

        public IReadOnlyReactiveProperty<bool> Selected => _selected;
        private readonly ReactiveProperty<bool> _selected = new ReactiveProperty<bool>();

        // 選択可能か
        public IReadOnlyReactiveProperty<bool> Selectable => _selectable;
        private readonly ReactiveProperty<bool> _selectable = new ReactiveProperty<bool>();

        // 使用ロック状態
        public IReadOnlyReactiveProperty<LockUseState> LockUseStatus => _lockUseStatus;

        private readonly ReactiveProperty<LockUseState> _lockUseStatus =
            new ReactiveProperty<LockUseState>(LockUseState.Finish);

        private string _commandName;

        public CommandCardModel(int id, string commandName, int effectNum, ICommandEffect commandEffect)
        {
            Id = id;
            _commandName = commandName;
            EffectNum = effectNum;
            CommandEffect = commandEffect;
            Description = "コマンドカードの説明が入る";
            _visible.Value = false;
        }

        public void SetPosition(Vector2Int position)
        {
            _position.Value = position;
            _commandName = position.ToString();
        }

        public void SetVisible(bool visible)
        {
            _visible.Value = visible;
        }

        public void SetSelected(bool isSelected)
        {
            _selected.Value = isSelected;
        }

        public void Reset()
        {
            _visible.Value = false;
            _selected.Value = false;
            _selectable.Value = false;
        }

        /// <summary>
        /// 選択可能かの設定
        /// </summary>
        /// <param name="selectable"></param>
        public void SetSelectable(bool selectable)
        {
            _selectable.Value = selectable;
        }

        /// <summary>
        /// 選択制限をセット
        /// </summary>
        public void LockUse(int lockTurnCount = 1)
        {
            _lockUseStatus.Value = new LockUseState(lockTurnCount, true);
        }

        /// <summary>
        /// 選択制限を解除
        /// </summary>
        public void UnlockUse()
        {
            // 効果切れ状態に
            _lockUseStatus.Value = LockUseState.Finish;
        }

        public void Use()
        {
            if (!_visible.Value)
            {
                return;
            }

            _onUse.OnNext(Unit.Default);
        }

        /// <summary>
        /// ターンを進める
        /// </summary>
        public void CountTurn()
        {
            // 付与ターンはデクリメントしない.
            if (_lockUseStatus.Value.IsStartTurn && _lockUseStatus.Value.Avairable)
            {
                _lockUseStatus.Value = new LockUseState(_lockUseStatus.Value.RemainingCount);
                return;
            }

            // 継続ターン数を減らす
            if (_lockUseStatus.Value.Avairable)
            {
                _lockUseStatus.Value--;
            }
        }

        public bool Equals(CommandCardModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Equals(CommandEffect, other.CommandEffect);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommandCardModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (CommandEffect != null ? CommandEffect.GetHashCode() : 0);
            }
        }
    }
}
