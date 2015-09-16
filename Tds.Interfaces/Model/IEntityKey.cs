namespace Tds.Interfaces.Model
{
    public interface IEntityKey
    {
        string Name { get; set; }

        object Value { get; set; }
    }
}
