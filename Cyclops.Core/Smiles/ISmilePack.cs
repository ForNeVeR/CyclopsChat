namespace Cyclops.Core.Smiles
{
    public interface ISmilePack
    {
        ISmilePackMeta Meta { get; }
        ISmile[] Smiles { get; }
    }
}