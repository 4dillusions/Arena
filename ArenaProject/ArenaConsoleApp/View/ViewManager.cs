using ArenaEngine.Controller;

namespace ArenaConsoleApp.View;

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