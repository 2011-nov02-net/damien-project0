namespace StoreManager.Library.Entity
{
    public class IdGenerator
    {
        private long _id = 0;
        
        public IdGenerator(long startingId) => _id = startingId;

        /// <summary>
        /// Generates an id
        /// </summary>
        /// <returns>A new id</returns>
        public long NextId() => _id++;
    }
}
