namespace AiConsole.UI;

public static class ConsoleInterface
{
    public static void ShowHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("""
         ╔══════════════════════════════════════════╗
         ║                                          ║
         ║                A.$.A                     ║
         ║          PERSONAL ASSISTANT              ║
         ║                                          ║
         ╚══════════════════════════════════════════╝
        """);

        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  ● Sistema iniciado");
        Console.ResetColor();

        Console.WriteLine();
    }

    public static void ShowUserMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Você: ");
        Console.ResetColor();

        Console.WriteLine(message);
        Console.WriteLine();
    }

    public static void ShowJarvisMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("JARVIS: ");
        Console.ResetColor();

        Console.WriteLine(message);
        Console.WriteLine();
    }

    public static void ShowListening()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  ● Ouvindo...");
        Console.ResetColor();
    }

    public static void ShowThinking()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("  ◉ Processando...");
        Console.ResetColor();
    }

    public static void ShowPrompt()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Você > ");
        Console.ResetColor();
    }
}