namespace GameZone.Services
{
	public interface IGamesService
	{
		IEnumerable<Game> GetAll();
		Game? GetById(int id);
		Task Create(CreateGameFormViewModel viewModel);
		Task<Game?> Edit(EditGameFormViewModel viewModel);
		bool Delete(int id);
	}
}
