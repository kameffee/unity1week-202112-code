namespace Unity1week202112.Domain.Command
{
    public class CommandEffectTypeEntity
    {
        
        public int Id { get; }
        public string EffectName { get; }
        
        public string Description { get; }

        public CommandEffectTypeEntity(int id, string effectName, string description)
        {
            Id = id;
            EffectName = effectName;
            Description = description;
        }
    }
}
