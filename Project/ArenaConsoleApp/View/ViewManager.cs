/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Controller;

namespace App4di.Dotnet.ArenaConsoleApp.View;

public class ViewManager
{
    private readonly Terminal terminal;
    private readonly Arena arena;

    public ViewManager(Terminal terminal, Arena arena)
    {
        this.terminal = terminal;
        this.arena = arena;
    }
        
    public void ShowTerminal(uint maxHeroCount)
    {
        arena.ArenaHeroesCount = maxHeroCount;
        arena.OnLogMessage += terminal.OnLogMessage!;
        arena.GameLoop();

        terminal.WaitForUserInput();
    }
}
