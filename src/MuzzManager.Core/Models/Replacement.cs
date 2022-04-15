namespace MuzzManager.Core.Models
{
	public class Replacement
	{
		public string Description { get; set; }

		public ReplacementScope Scope { get; set; }

		public string Pattern { get; set; }

		public string ReplacementValue { get; set; }
	}
}