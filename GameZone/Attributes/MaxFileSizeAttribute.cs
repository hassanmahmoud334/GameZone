namespace GameZone.Attributes
{
	public class MaxFileSizeAttribute :ValidationAttribute
	{
		private readonly int _maxFileSizeInBytes;
		public MaxFileSizeAttribute(int maxFileSizeInBytes)
		{
			_maxFileSizeInBytes = maxFileSizeInBytes;
		}
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is IFormFile file)
			{
				if (file.Length > _maxFileSizeInBytes)
				{
					return new ValidationResult($"The file size should be less than {_maxFileSizeInBytes / 1024 / 1024}MB!");
				}
			}
			return ValidationResult.Success;
		}
	}
}
