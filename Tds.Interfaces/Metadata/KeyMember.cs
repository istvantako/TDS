using YAXLib;
namespace Tds.Interfaces.Metadata
{
    public class KeyMember
    {
        [YAXAttributeForClass(), YAXSerializeAs("Order")]
        public int Sequence { get; set; }

        [YAXAttributeForClass()]
        public string Name { get; set; }

        public KeyMember()
        {
            Sequence = 0;
            Name = string.Empty;
        }

        public KeyMember(string name, int sequence)
        {
            Name = name;
            Sequence = sequence;
        }
    }
}
