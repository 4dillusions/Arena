using ArenaEngine.Controller;

namespace ArenaConsoleApp.View
{
    public class ViewManager
    {
        private readonly Terminal terminal;
        private readonly LimeArena arena;

        public ViewManager(Terminal terminal, LimeArena arena)
        {
            this.terminal = terminal;
            this.arena = arena;
        }
        
        public void ShowTerminal(uint maxHeroCount)
        {
            arena.ArenaHeroesCount = maxHeroCount;
            arena.OnLogMessage += terminal.OnLogMessage;
            arena.GameLoop();

            terminal.WaitForUserInput();
        }
    }
}
