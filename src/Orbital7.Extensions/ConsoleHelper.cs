namespace System;

public enum ConsoleEnterOrEscKeyResult
{
    Enter,
    Escape,
}

public static class ConsoleHelper
{
    public static ConsoleEnterOrEscKeyResult PressEnterOrEscKey(bool showInstructions = true, string enterVerb = "continue", string escVerb = "exit")
    {
        if (showInstructions)
        {
            Console.WriteLine();
            Console.WriteLine("Press ENTER to {0} or ESC to {1}", enterVerb, escVerb);
        }

        var key = Console.ReadKey();
        while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
        {
            key = Console.ReadKey();
        }

        if (key.Key == ConsoleKey.Escape)
            return ConsoleEnterOrEscKeyResult.Escape;
        else
            return ConsoleEnterOrEscKeyResult.Enter;
    }

    public static void WriteExceptionLine(Exception ex, string prefix = "ERROR: ")
    {
        Console.WriteLine();
        Console.WriteLine((prefix + ex.FlattenMessages() + ex.StackTrace).Trim());
    }

    public static void PressKeyToContinue(string verb = "continue")
    {
        Console.WriteLine();
        Console.WriteLine("Press a key to {0}", verb);
        Console.ReadKey();
    }

    public static string GetArgDirectiveValue(string[] args, string argDirective, string argDirectivePrefix = "-")
    {
        bool found = false;

        foreach (string arg in args)
        {
            if (found && !arg.StartsWith(argDirectivePrefix))
                return arg;
            else if (found)
                break;

            if (arg.Equals(argDirective, StringComparison.CurrentCultureIgnoreCase))
                found = true;
        }

        return null;
    }
}
