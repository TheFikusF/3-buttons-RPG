using RPG.GUI;
using Terminal.Gui;

namespace RPG
{
    internal static class Program
    {
        static void Main()
        {
            Application.Run<GameWindow>();

            Application.Shutdown();
        }
    }
}