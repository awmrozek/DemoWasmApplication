using ImTransformBackend.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ImTransformBackend.Tests
{
    public class FileRepositoryTests
    {
        [Fact]
        public void GetWasmFiles_ReturnsOnlyWasmFiles()
        {
            using var temp = new TemporaryDirectory();

            File.WriteAllText(Path.Combine(temp.Path, "a.wasm"), "wasm");
            File.WriteAllText(Path.Combine(temp.Path, "notes.txt"), "text");
            File.WriteAllText(Path.Combine(temp.Path, "b.WASM"), "wasm");

            var repository = new FileRepository();

            var result = repository.GetWasmFiles(temp.Path);

            Assert.Equal(
                new[] { "a.wasm", "b.WASM" },
                result.Select(file => file.Name).OrderBy(name => name).ToArray());
        }

        [Fact]
        public void GetWasmFiles_ReturnsEmptyForMissingFolder()
        {
            var repository = new FileRepository();
            var missingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            var result = repository.GetWasmFiles(missingPath);

            Assert.Empty(result);
        }

        [Fact]
        public void GetLatestFiles_ReturnsFilesNewestFirstAndRespectsCount()
        {
            using var temp = new TemporaryDirectory();

            string olderPath = Path.Combine(temp.Path, "older.txt");
            string newerPath = Path.Combine(temp.Path, "newer.txt");

            File.WriteAllText(olderPath, "old");
            File.WriteAllText(newerPath, "new");

            DateTime olderTime = DateTime.UtcNow.AddMinutes(-10);
            DateTime newerTime = DateTime.UtcNow;

            File.SetLastWriteTimeUtc(olderPath, olderTime);
            File.SetLastWriteTimeUtc(newerPath, newerTime);

            var repository = new FileRepository();

            var result = repository.GetLatestFiles(temp.Path, count: 1);

            var file = Assert.Single(result);
            Assert.Equal("newer.txt", file.Name);
            Assert.Equal(newerTime, file.LastModified);
        }

        private sealed class TemporaryDirectory : IDisposable
        {
            public string Path { get; } =
                System.IO.Path.Combine(
                    System.IO.Path.GetTempPath(),
                    Guid.NewGuid().ToString("N"));

            public TemporaryDirectory() => Directory.CreateDirectory(Path);

            public void Dispose()
            {
                if (Directory.Exists(Path))
                    Directory.Delete(Path, recursive: true);
            }
        }
    }

}
