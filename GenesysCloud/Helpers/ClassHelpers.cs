using System.Diagnostics;

namespace GenesysCloud.Helpers;

public static class ClassHelpers
{
    public static string GetMethodName(int stackFrameIndex = 2)
    {
        var stackTrace = new StackTrace();
        var stackFrame = stackTrace.GetFrame(stackFrameIndex);
        var methodBase = stackFrame?.GetMethod();
        
        return $"{methodBase?.ReflectedType?.Name}.{methodBase?.Name}";
    }
}