using CSharpVitamins;
using MassTransit;
using MassTransit.NewIdProviders;

namespace Orbital7.Extensions;

public static class GuidFactory
{
    // Source: https://github.com/csharpvitamins/CSharpVitamins.ShortGuid
    public const int SHORT_GUID_LENGTH = 22;

    static GuidFactory()
    {
        NewId.SetProcessIdProvider(new CurrentProcessIdProvider());
    }

    public static Guid Next()
    {
        return NewId.NextGuid();
    }

    public static string NextShortString()
    {
        return ToShortString(Next());
    }

    public static Guid NextSequential()
    {
        return NewId.NextSequentialGuid();
    }

    public static string NextSequentialShortString()
    {
        return ToShortString(NextSequential());
    }

    public static Guid FromString(
        string guid)
    {
        if (guid.Length == SHORT_GUID_LENGTH)
        {
            return ShortGuid.Decode(guid);
        }
        else
        {
            return Guid.Parse(guid);
        }
    }

    public static string ToShortString(
        Guid guid)
    {
        return ShortGuid.Encode(guid);
    }
}
