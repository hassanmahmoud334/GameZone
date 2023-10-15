using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
	public class DevicesService : IDevicesService
	{
		private readonly ApplicationDbContext _dbContext;
		public DevicesService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IEnumerable<SelectListItem> GetSelectList()
		{
			return _dbContext.Devices
				.Select(d => new SelectListItem { Text = d.Name, Value = d.Id.ToString() })
				.OrderBy(d => d.Text)
				.AsNoTracking()
				.ToList();
		}
	}
	
}
