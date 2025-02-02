namespace ShevkunenkoSite.Models.Infrastructure;

public class ModelStateTransferValue
{
    public string Key { get; set; } = string.Empty;
    public string AttemptedValue { get; set; } = string.Empty;
    public object RawValue { get; set; } = new object();
    public ICollection<string> ErrorMessages { get; set; } = [];
}
