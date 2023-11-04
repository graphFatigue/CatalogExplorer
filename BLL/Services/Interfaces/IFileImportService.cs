namespace BLL.Services.Interfaces
{
    public interface IFileImportService
    {
        Task ImportFromDirectoryAsync(string directoryPath);
        Task ImportFromFileAsync(Stream fileStream);
        Task ExportToTextFileAsync(string filePath);
    }
}
