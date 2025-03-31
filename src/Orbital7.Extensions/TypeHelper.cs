namespace Orbital7.Extensions;

public static class TypeHelper
{
    public static string GetClassNameFromAssemblyQualifiedTypeName(
        string assemblyQualifiedTypeName)
    {
        if (assemblyQualifiedTypeName.HasText())
        {
            var frags1 = assemblyQualifiedTypeName.Parse(",");
            if (frags1.Length > 0)
            {
                var frags2 = frags1[0].Parse(" ");
                if (frags2.Length > 0)
                {
                    return frags2.Last();
                }
            }
        }

        return null;
    }
}
