/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
namespace App4di.Dotnet.ArenaConsoleApp.View;

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
