using System;
using Unity1week202112.Domain.Command;

namespace Unity1week202112.Domain.Deck
{
    public class CommandCardEntity : IEquatable<CommandCardEntity>
    {
        public int Id { get; }

        public CommandEffectType EffectType { get; }

        public int Value { get; }

        public CommandCardEntity(int id, CommandEffectType effectType, int value)
        {
            Id = id;
            EffectType = effectType;
            Value = value;
        }

        public CommandCardEntity(int id, CommandCardEntity baseEntity)
        {
            Id = id;
            EffectType = baseEntity.EffectType;
            Value = baseEntity.Value;
        }

        public bool Equals(CommandCardEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && EffectType == other.EffectType && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommandCardEntity)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (int)EffectType;
                hashCode = (hashCode * 397) ^ Value;
                return hashCode;
            }
        }
    }
}
