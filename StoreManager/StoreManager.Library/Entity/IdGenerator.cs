namespace StoreManager.Library.Entity
{
    public class IdGenerator
    {
        private int _id = 0;
        
        public IdGenerator(int startingId) => _id = startingId;

        /// <summary>
        /// Generates an id
        /// </summary>
        /// <returns>A new id</returns>
        public int NextId() => _id++;
    }
}
