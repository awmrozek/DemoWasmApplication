namespace ImTransformBackend.Services
{
    public record FileItem(string Name, string FullPath, DateTime LastModified);

    public class FileRepository
    {
        public IReadOnlyList<FileItem> GetLatestFiles(string folder, int count = 10)
        {
            if (!Directory.Exists(folder))
                return [];

            return Directory.EnumerateFiles(folder)
                .Select(path => new FileInfo(path))
                .OrderByDescending(file => file.LastWriteTimeUtc)
                .Take(count)
                .Select(file => new FileItem(
                    file.Name,
                    file.FullName,
                    file.LastWriteTimeUtc))
                .ToList();
        }

        public IReadOnlyList<FileItem> GetWasmFiles(string folder)
        {
            if (!Directory.Exists(folder))
                return [];

            return Directory.EnumerateFiles(
                    folder, "*.wasm", SearchOption.TopDirectoryOnly)
                .Select(path => new FileInfo(path))
                .OrderBy(file => file.Name)
                .Select(file => new FileItem(
                    file.Name,
                    file.FullName,
                    file.LastWriteTimeUtc))
                .ToList();
        }

        public IReadOnlyList<FileItem> GetWasmFiles()
        {
            return GetWasmFiles(AppContext.BaseDirectory + "Native");
        }

        public IReadOnlyList<FileItem> GetLatestFiles()
        {
            return GetLatestFiles(AppContext.BaseDirectory + "Images");
        }
    }

}
