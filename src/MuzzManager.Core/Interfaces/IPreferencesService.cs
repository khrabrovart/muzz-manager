namespace MuzzManager.Core.Interfaces
{
	using Models;

	public interface IPreferencesService
	{
		UserPreferences SelectUserPreferences();

		string SelectDirectory(string caption);
	}
}