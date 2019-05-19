namespace Prototype.Models.ViewModels
{
	public interface IViewModelFactory
	{
		#region Methods

		IViewModel Create();

		#endregion
	}
}