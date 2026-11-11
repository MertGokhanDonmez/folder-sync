using System.Security.Cryptography;

class FolderSynchronizer
{
	private readonly string sourcePath;
	private readonly string replicaPath;
	private readonly Logger logger;

	public FolderSynchronizer(string sourcePath, string replicaPath, Logger logger)
	{
		this.sourcePath = sourcePath;
		this.replicaPath = replicaPath;
		this.logger = logger;
	}

	public void Synchronize()
	{
		IEnumerable<string> sourceFiles = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories);
		IEnumerable<string> replicaFiles = Directory.GetFiles(replicaPath, "*", SearchOption.AllDirectories);
		IEnumerable<string> replicaDirectories = Directory.GetDirectories(replicaPath, "*", SearchOption.AllDirectories);

		foreach (var item in replicaFiles)
		{
			string sourceFilePath = Path.Combine(sourcePath, Path.GetRelativePath(replicaPath, item));
			if (!File.Exists(sourceFilePath))
			{
				File.Delete(item);
				logger.Log($"File Deleted: {item}");
			}
		}

		foreach (var item in replicaDirectories)
		{
			string sourceDirectoryPath = Path.Combine(sourcePath, Path.GetRelativePath(replicaPath, item));
			if (!Directory.Exists(sourceDirectoryPath) && Directory.Exists(item))
			{
				Directory.Delete(item, true);
				logger.Log($"Directory Deleted: {item}");
			}
		}

		foreach (var item in sourceFiles)
		{
			string replicaFilesPath = Path.Combine(replicaPath, Path.GetRelativePath(sourcePath, item));
			string? replicaDirectoryName = Path.GetDirectoryName(replicaFilesPath);
			if (replicaDirectoryName != null && !Directory.Exists(replicaDirectoryName))
			{
				Directory.CreateDirectory(replicaDirectoryName);
				logger.Log($"Directory Created: {replicaDirectoryName}");
			}
			if (!File.Exists(replicaFilesPath) || (GetHash(item) != GetHash(replicaFilesPath)))
			{
				File.Copy(item, replicaFilesPath, true);
				logger.Log($"File Copied: {item}");
			}
		}
	}

	private static string GetHash(string path)
	{
		using MD5 md5 = MD5.Create();
		using FileStream file = File.OpenRead(path);
		return Convert.ToHexString(md5.ComputeHash(file));
	}
}
