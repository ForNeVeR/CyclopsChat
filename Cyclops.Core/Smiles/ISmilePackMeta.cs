namespace Cyclops.Core.Smiles
{
    public interface ISmilePackMeta
    {
        string Name { get; set; }
        string Version { get; set; }
        string Creation { get; set; }
        string Description { get; set; }
        string Home { get; set; }
        string Author { get; set; }
    }
}