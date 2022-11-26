namespace ArenaConsoleApp.View;

public class Terminal
{
    public void OnLogMessage(object sender, Tuple<string, ConsoleColor> args)
    {
        Console.ForegroundColor = args.Item2;
        Console.WriteLine(args.Item1);
    }

    public void WaitForUserInput()
    {
        Console.ResetColor();
        Console.ReadKey();
    }
}