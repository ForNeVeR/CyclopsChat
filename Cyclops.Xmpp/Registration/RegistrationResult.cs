namespace Cyclops.Xmpp.Registration;

public class RegistrationResult
{
    public string? ErrorMessage { get; }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public RegistrationResult(string? errorMessage = null)
    {
        ErrorMessage = errorMessage;
    }
}
