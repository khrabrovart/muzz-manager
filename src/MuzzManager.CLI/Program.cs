namespace MuzzManager.CLI
{
    using Microsoft.Extensions.DependencyInjection;
    using Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            var startup = new Startup();

            var mainService = startup.Provider.GetRequiredService<IMainMusicService>();
            mainService.Start();
        }
    }
}