using Cyclops.Configuration;

namespace Cyclops.Core.Resource
{
    public class ChatObjectsValidator : IChatObjectsValidator
    {
        #region Implementation of IChatObjectsValidator

        public bool ValidateConfig(ConnectionConfig config)
        {
            //TODO:
            return true;
        }

        public bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            if (name.Length > 100)
                return false;

            return true;
        }

        public bool ValidateHost(string host)
        {
            //TODO:
            return true;
        }

        #endregion
    }
}
