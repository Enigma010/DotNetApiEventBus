namespace DotNetApiEventBus
{
    public interface IDddConfig
    {
        string Domain { get; set; }
        string SubDomain { get; set; }
    }
}