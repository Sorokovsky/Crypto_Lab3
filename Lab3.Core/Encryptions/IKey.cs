namespace Lab3.Core.Encryptions;

public interface IKey
{
    public string ToString();

    public IKey FromJson(string json);
}