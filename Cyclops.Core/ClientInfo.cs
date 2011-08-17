namespace Cyclops.Core
{
    public class ClientInfo
    {
        public ClientInfo(string os, string version, string client)
        {
            Os = os;
            Version = version;
            Client = client;
        }

        public string Os { get; private set; }
        public string Version { get; private set; }
        public string Client { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Client, Version);
        }
    }
}