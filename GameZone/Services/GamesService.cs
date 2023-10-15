
namespace GameZone.Services
{
	public class GamesService : IGamesService
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _env;
		private readonly string _imagesPath;
		public GamesService(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_env = env;
			_imagesPath = $"{_env.WebRootPath}{FileSettings.GamesImagesPath}";
		}
		public IEnumerable<Game> GetAll()
		{
			return _dbContext.Games
				.Include(g => g.Category)
				.Include(g => g.Devices)
				.ThenInclude(gd => gd.Device)
				.AsNoTracking()
				.ToList();
		}
        public Game? GetById(int id)
        {
			return _dbContext.Games
				.Include(g => g.Category)
				.Include(g => g.Devices)
				.ThenInclude(gd => gd.Device)
				.AsNoTracking()
				.SingleOrDefault(g => g.Id == id);
        }
        public async Task Create(CreateGameFormViewModel model)
		{
			var coverName = await SaveCover(model.Cover);

			Game game = new()
			{
				Name = model.Name,
				Description = model.Description,
				Cover = coverName,
				CategoryId = model.CategoryId,
				Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
			};

			_dbContext.Add(game);
			_dbContext.SaveChanges();
		}

		public async Task<Game?> Edit(EditGameFormViewModel viewModel)
		{
			var game=_dbContext.Games.
				Include(g => g.Devices).
				SingleOrDefault(g => g.Id == viewModel.Id);
			if(game is null)
			{
				return null;
			}
			var hasNewCover= viewModel.Cover is not null;
			var oldCover = game.Cover;
			game.Name = viewModel.Name;
			game.Description = viewModel.Description;
			game.CategoryId = viewModel.CategoryId;
			game.Devices = viewModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();
			if(hasNewCover)
			{
				game.Cover = await SaveCover(viewModel.Cover!);
			}
			var effectedRows= _dbContext.SaveChanges();
			if(effectedRows > 0)
			{
				if (hasNewCover)
				{
					var cover = Path.Combine(_imagesPath, oldCover);
					File.Delete(cover);
				}
				return game;
			}
			else
			{

				var cover = Path.Combine(_imagesPath, game.Cover);
				File.Delete(cover);
				return null;
			}
		}
		public bool Delete(int id)
		{
			var isDeleted = false;
			var game = _dbContext.Games.Find(id);
			if(game is null) 
				return false;
			
			_dbContext.Games.Remove(game);
			var effectedRows = _dbContext.SaveChanges();
			if(effectedRows > 0)
			{
				var cover = Path.Combine(_imagesPath, game.Cover);
				File.Delete(cover);
				isDeleted = true;
			}
			
			return isDeleted;
		}
		private async Task<string> SaveCover(IFormFile cover)
		{
			var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
			var path = Path.Combine(_imagesPath, coverName);
			using var stream = File.Create(path);
			await cover.CopyToAsync(stream);
			return coverName;
		}

		
	}
	
}
