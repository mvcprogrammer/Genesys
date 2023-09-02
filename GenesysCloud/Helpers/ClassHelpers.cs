using System.Diagnostics;

namespace GenesysCloud.Helpers;

public static class ClassHelpers
{
    public static string GetCallingMethodName()
    {
        var stackTrace = new StackTrace();
        var stackFrame = stackTrace.GetFrame(2);
        var methodBase = stackFrame?.GetMethod();
        
        return $"{methodBase?.ReflectedType?.Name}.{methodBase?.Name}";
    }
}