namespace ArkhManufacturing.Library
{
    public class IdGenerator
    {
        private long _id = 0;

        public long NextId() => _id++;
    }
}
