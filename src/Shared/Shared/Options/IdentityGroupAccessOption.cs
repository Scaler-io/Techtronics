namespace Shared.Options;
public class IdentityGroupAccessOption
{
    public const string OptionName = "IdentityGroupAccess";
    public string Authority { get; set; }
    public string Audience { get; set; }
}
