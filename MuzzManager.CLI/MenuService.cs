namespace MuzzManager.CLI
{
    using System;
    using Interfaces;

    public class MenuService : IMenuService
    {
        public int OpenMenu(string title, string[] items)
        {
            var selectedIndex = 0;
            var keyPressed = default(ConsoleKey);

            while (keyPressed != ConsoleKey.Enter)
            {
                DrawMenu(title, items, selectedIndex);
                keyPressed = Console.ReadKey(true).Key;

                switch (keyPressed)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(items.Length - 1, selectedIndex + 1);
                        break;
                }
            }

            Console.Clear();
            return selectedIndex;
        }

        public void OpenMessage(string messageText)
        {
            OpenMenu(messageText, new[] { "Ok" });
        }

        private void DrawMenu(string title, string[] items, int selectedIndex)
        {
            Console.Clear();

            if (!string.IsNullOrWhiteSpace(title))
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(title);
                Console.ResetColor();
            }

            for (var i = 0; i < items.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(items[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(items[i]);
                }
            }
        }
    }
}