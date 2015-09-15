namespace Tds.Interfaces.Database
{
    public interface IEntityKey
    {
        public virtual string Name { get; set; }
        public virtual object Value { get; set; }
    }
}
