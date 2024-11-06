using System;
using System.Collections.Generic;
using System.Linq;
using ErosScriptingEngine.Component;

namespace ErosScriptingEngine.Util
{
    public class FileExtensionValidator : ErosValidationComponent<string>
    {
        private readonly List<string> allowedExtensions;

        public FileExtensionValidator(params string[] allowedExtensions)
        {
            this.allowedExtensions = allowedExtensions.ToList();
        }

        public void Validate(string fileName)
        {
            string fileExtension = GetFileExtension(fileName);

            if (fileExtension == null || !allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException(
                    $"Invalid file extension: .{fileExtension}. Allowed extensions are: {string.Join(", ", allowedExtensions)}");
            }
        }

        private string GetFileExtension(string fileName)
        {
            int dotIndex = fileName.LastIndexOf('.');
            if (dotIndex > 0 && dotIndex < fileName.Length - 1)
            {
                return fileName.Substring(dotIndex + 1).ToLower();
            }

            return null;
        }
    }
}