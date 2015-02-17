namespace Tds.Tests.Model.Entities
{
    public class ClonableEntityBase<T>
    {
        public T ShallowCopy()
        {
            return (T)this.MemberwiseClone();
        }
    }
}
