namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public abstract class Identifiable
    {
        // TODO: Add comment here
        public long Id { get; private set; }
        // TODO: Add comment here
        public Identifiable(IdGenerator idGenerator) {
            Id = idGenerator.NextId();
        }
    }
}
