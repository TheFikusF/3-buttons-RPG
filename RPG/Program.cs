using RPG.GUI;
using Terminal.Gui;

namespace RPG
{
    internal static class Program
    {
        static void Main()
        {
            Application.Run<GameWindow>();

            //Console.WriteLine($"Username: {((GameWindow)Application.Top).usernameText.Text}");

            // Before the application exits, reset Terminal.Gui for clean shutdown
            Application.Shutdown();
        }
    }
}