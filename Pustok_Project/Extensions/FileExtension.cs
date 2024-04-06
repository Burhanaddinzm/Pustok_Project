namespace Pustok_Project.Extensions
{
    public static class FileExtension
    {
        public static async Task<string> SaveFileAsync(this IFormFile file, string root, string client, string assets, string folderType, string folderName)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string path = Path.Combine(root, client, assets, folderType, folderName, uniqueFileName);

            using FileStream fs = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(fs);

            return uniqueFileName;
        }

        public static bool CheckFileType(this IFormFile file, string fileType)
        {
            if (file.ContentType.Contains(fileType))
            {
                return true;
            }
            return false;
        }

        public static bool CheckFileSize(this IFormFile file, int megabytes)
        {
            if (file.Length / 1024 / 1024 >= megabytes)
            {
                return false;
            }
            return true;
        }

        public static void DeleteFile(this IFormFile file, string root, string client, string assets, string folderType, string folderName, string fileName)
        {
            string path = Path.Combine(root, client, assets, folderType, folderName, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static FileValidationResult ValidateFile(this IFormFile file)
        {
            if (!file.CheckFileSize(2))
            {
                return new FileValidationResult(false, "File size can't exceed 2 MB!");
            }

            if (!file.CheckFileType("image"))
            {
                return new FileValidationResult(false, "File type is invalid!");
            }

            return new FileValidationResult(true);
        }

        public class FileValidationResult
        {
            public bool IsValid { get; }
            public string ErrorMessage { get; }

            public FileValidationResult(bool isValid, string errorMessage = "")
            {
                IsValid = isValid;
                ErrorMessage = errorMessage;
            }
        }
    }
}
