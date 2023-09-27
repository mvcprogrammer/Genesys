namespace GenesysCloud.Helpers.Logger.Console;
using System;    

public class ConsoleLogger : ILogger
{
    private readonly List<string> _messageList = new();

    public void LogSuccess(string successMessage)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine($"** SUCCESS ** {successMessage}");
    }

    public void LogError(string errorMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        WriteLine($"** ERROR **** {errorMessage}");
    }

    public void LogInfo(string infoMessage)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        WriteLine($"** INFO ***** {ClassHelpers.GetMethodName()} : {infoMessage}");
    }

    public void LogMethodBegin()
    {
        Console.ForegroundColor = ConsoleColor.White;
        WriteLine($"** METHOD *** {ClassHelpers.GetMethodName()}");
    }

    public void LogDebug(string debugMessage)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteLine($"** DEBUG **** {ClassHelpers.GetMethodName()} : {debugMessage}");
    }

    private void WriteLine(string message)
    {
        var line = $"{DateTime.Now:MM/dd/yyyy HH:mm:ss.fff zzz} - {message}";
        _messageList.Add(line);
        Console.WriteLine(line);
    }

    public void DumpLogFile(string logFilePath)
    {
        try
        {
            var fileInfo = new FileInfo(logFilePath);
            using var streamWriter = fileInfo.CreateText();
            foreach (var message in _messageList) { streamWriter.Write($"{message}\r\n"); }
            streamWriter.Close();
        }
        catch (Exception)
        {
            // Ignored
        }
    }
}