using Cyclops.Core.Configuration;

namespace Cyclops.Core
{
    public interface IChatObjectsValidator
    {
        bool ValidateConfig(ConnectionConfig config);
        bool ValidateName(string name);
        bool ValidateHost(string host);
    }
}