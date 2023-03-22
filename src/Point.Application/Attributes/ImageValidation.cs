namespace Point.Application.Attributes
{
    public class ImageValidation : ValidationAttribute
    {
        private readonly int _maxImageCount;
        private readonly int _maxImageSize;
        private readonly List<string> _imageExtentions;

        public ImageValidation()
        {
            _maxImageCount = 5;
            _maxImageSize = 20971520;
            _imageExtentions = new List<string> { ".jpeg", ".jpg", ".png" };
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null && files.Count <= _maxImageCount)
            {
                foreach (var file in files)
                {
                    if (file == null)
                    {
                        continue;
                    }

                    //Checking image format
                    var fileExtention = Path.GetExtension(file.FileName).ToLower();
                    if (!_imageExtentions.Contains(fileExtention))
                    {
                        return new ValidationResult($"Image {file.FileName} has invalid format");
                    }

                    // Image size validation
                    if (file.Length > _maxImageSize)
                    {
                        return new ValidationResult("Image size exceeds 20MB");
                    }

                    // Image format validation
                    if (!file.ContentType.StartsWith("image/"))
                    {
                        return new ValidationResult("Invalid image format");
                    }

                    return ValidateContentType(file);
                }
            }
            else
                return new ValidationResult("Invalid count of files");

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateContentType(IFormFile file)
        {
            switch (file.ContentType)
            {
                case "image/jpg":
                    {
                        return !IsJpg(file)
                           ? new ValidationResult($"Image {file.FileName} has invalid JPG format")
                           : ValidationResult.Success;
                    }
                case "image/jpeg":
                    {
                        return !IsJpeg(file)
                            ? new ValidationResult($"Image {file.FileName} has invalid JPEG format")
                            : ValidationResult.Success;
                    }
                case "image/png":
                    {
                        return !IsPng(file)
                            ? new ValidationResult($"Image {file.FileName} has invalid JPEG format")
                            : ValidationResult.Success;
                    }
                default:
                    return new ValidationResult("Invalid file format. Only .JPEG/.JPG/.SVG/.GIF formats are allowed.");
            }
        }

        private bool IsJpg(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                byte[] header = new byte[4];
                stream.Read(header, 0, 4);
                return header.SequenceEqual(new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 })
                    || header.SequenceEqual(new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 });
            }
        }

        private bool IsJpeg(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                byte[] header = new byte[2];
                stream.Read(header, 0, 2);
                return header.SequenceEqual(new byte[] { 0xFF, 0xD8 });
            }
        }

        private bool IsPng(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                byte[] header = new byte[8];
                stream.Read(header, 0, 8);
                return header.SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
            }
        }
    }
}
