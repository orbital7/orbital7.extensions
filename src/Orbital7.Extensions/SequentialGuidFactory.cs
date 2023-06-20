namespace System;

public static class SequentialGuidFactory
{
    private static long _lastTicks = -1;

    // Source: https://stackoverflow.com/questions/1752004/sequential-guid-generator
    public static Guid NewGuid()
    {
        long ticks = DateTime.UtcNow.Ticks;

        if (ticks <= _lastTicks)
        {
            ticks = _lastTicks + 1;
        }

        _lastTicks = ticks;

        byte[] ticksBytes = BitConverter.GetBytes(ticks);

        Array.Reverse(ticksBytes);

        Guid myGuid = new Guid();
        byte[] guidBytes = myGuid.ToByteArray();

        Array.Copy(ticksBytes, 0, guidBytes, 10, 6);
        Array.Copy(ticksBytes, 6, guidBytes, 8, 2);

        return new Guid(guidBytes);
    }
}
