using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Lab3.Core.Encryptions;

public class EncryptionsDictionary
{
    private static EncryptionsDictionary? _instance;

    private readonly IDictionary<string, IEncryption> _dictionary = new ConcurrentDictionary<string, IEncryption>();

    private EncryptionsDictionary()
    {
        var encryptions = CreateEncryptions();
        SaveEncryptions(encryptions);
    }

    public static EncryptionsDictionary Instance => GetInstance();
    public IReadOnlyDictionary<string, IEncryption> All => new ReadOnlyDictionary<string, IEncryption>(_dictionary);

    private static EncryptionsDictionary GetInstance()
    {
        if (_instance is null) _instance = new EncryptionsDictionary();
        return _instance;
    }

    public bool TryGet(string key, out IEncryption value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    private static List<IEncryption> CreateEncryptions()
    {
        var interfaceType = typeof(IEncryption);

        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => interfaceType.IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false })
            .Select(type =>
            {
                try
                {
                    return Activator.CreateInstance(type) as IEncryption;
                }
                catch
                {
                    var ctor = type.GetConstructors()
                        .FirstOrDefault(c => c.GetParameters().All(p => p.IsOptional));

                    if (ctor != null)
                    {
                        var defaultParams = ctor.GetParameters().Select(_ => Type.Missing).ToArray();
                        return ctor.Invoke(defaultParams) as IEncryption;
                    }

                    return null;
                }
            })
            .Where(e => e is not null)
            .ToList()!;
    }


    private void SaveEncryptions(IEnumerable<IEncryption> encryptions)
    {
        foreach (var encryption in encryptions)
        {
            var key = encryption.ToString();
            _dictionary[key] = encryption;
        }
    }
}