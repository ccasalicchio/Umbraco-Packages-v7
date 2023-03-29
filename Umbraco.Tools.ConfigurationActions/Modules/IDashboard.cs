namespace Umbraco.Tools.ConfigurationActions.Modules
{
    public interface IDashboard
    {
        string Alias { get; }
        string[] Areas { get; }
        string TabCaption { get; }
        string Control { get; }
        string[] AccessDeny { get; }
        string[] AccessGrant { get; }
    }
}
