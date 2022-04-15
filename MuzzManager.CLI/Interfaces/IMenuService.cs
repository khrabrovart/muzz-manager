namespace MuzzManager.CLI.Interfaces
{
    public interface IMenuService
    {
        int OpenMenu(string title, string[] items);

        void OpenMessage(string messageText);
    }
}