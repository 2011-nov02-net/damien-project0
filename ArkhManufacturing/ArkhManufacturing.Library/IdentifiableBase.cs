namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public abstract class IdentifiableBase
    {
        // TODO: Add comment here
        public long Id { get; private set; }
        // TODO: Add comment here
        public IdentifiableBase(IdGenerator idGenerator) {
            Id = idGenerator.NextId();
        }
    }
}
