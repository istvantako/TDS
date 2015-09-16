namespace Tds.Interfaces.Metadata
{
    public interface IKeyMember
    {
        int Sequence { get; set; }

        string Name { get; set; }
    }
}
