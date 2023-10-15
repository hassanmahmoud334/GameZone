namespace GameZone.Attributes
{
	public class AllowedExtensionsAttribute : ValidationAttribute
	{
		private readonly string[] _extensions;
		public AllowedExtensionsAttribute(string extensions)
		{
			_extensions = extensions.Split(',');
		}
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is IFormFile file)
			{
				var extension = Path.GetExtension(file.FileName);
				if (!_extensions.Contains(extension.ToLower()))
				{
					return new ValidationResult($"This file extension is not allowed!");
				}
			}
			return ValidationResult.Success;
		}
	}
}
