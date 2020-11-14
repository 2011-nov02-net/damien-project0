using ArkhManufacturing.Library.CreationData;

namespace ArkhManufacturing.Library.Creator
{
    public interface ICreator<T>
        where T : Identifiable
    {
        T Create(ICreationData creationData);
    }
}
