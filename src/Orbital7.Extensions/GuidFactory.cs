using CSharpVitamins;
using MassTransit;

namespace System;

public static class GuidFactory
{
    // Source: https://github.com/csharpvitamins/CSharpVitamins.ShortGuid
    public const int SHORT_GUID_LENGTH = 22;

    public static Guid NextSequential()
    {
        return NewId.NextSequentialGuid();
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
