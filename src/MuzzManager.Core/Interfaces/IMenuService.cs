namespace MuzzManager.Core.Interfaces
{
    public interface IMenuService
    {
        int OpenMenu(string title, string[] items);

        void OpenMessage(string messageText);
    }
}