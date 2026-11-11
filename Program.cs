if (args.Length != 4)
{
	Console.WriteLine("Input error!");
	Console.WriteLine("Usage: <source> <replica> <interval> <log>");
	return;
}

string	sourcePath = args[0];
string	replicaPath = args[1];
string	intervalString = args[2];
string	logPath = args[3];

if (!int.TryParse(intervalString, out int intervalNumber) || intervalNumber <= 0)
{
	Console.WriteLine("Interval must be a positive number!");
	return;
}

if (!Directory.Exists(sourcePath))
{
	Console.WriteLine($"{sourcePath} source path does not exist!");
	return;
}

Directory.CreateDirectory(replicaPath);

IEnumerable<string> sourceFiles = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories);
IEnumerable<string> replicaFiles = Directory.GetFiles(replicaPath, "*", SearchOption.AllDirectories);
IEnumerable<string> replicaDirectories = Directory.GetDirectories(replicaPath, "*", SearchOption.AllDirectories);

foreach (var item in replicaFiles)
{
	string sourceFilePath = Path.Combine(sourcePath, Path.GetRelativePath(replicaPath, item));
	if (!File.Exists(sourceFilePath))
	{
		File.Delete(item);
		Log($"File Deleted: {item}", logPath);
	}
}

foreach (var item in replicaDirectories)
{
	string sourceDirectoryPath = Path.Combine(sourcePath, Path.GetRelativePath(replicaPath, item));
	if (!Directory.Exists(sourceDirectoryPath) && Directory.Exists(item))
	{
		Directory.Delete(item,true);
		Log($"Directory Deleted: {item}", logPath);
	}
}

foreach (var item in sourceFiles)
{
	string replicaFilesPath = Path.Combine(replicaPath, Path.GetRelativePath(sourcePath, item));
	string? replicaDirectoryName = Path.GetDirectoryName(replicaFilesPath);
	if (replicaDirectoryName != null && !Directory.Exists(replicaDirectoryName))
	{
		Directory.CreateDirectory(replicaDirectoryName);
		Log($"Directory Created: {replicaDirectoryName}", logPath);
	}
	if (!File.Exists(replicaFilesPath) || (GetHash(item) != GetHash(replicaFilesPath)))
	{
		File.Copy(item, replicaFilesPath, true);
		Log($"File Copied: {item}", logPath);
	}
}

static string GetHash(string path)
{
    using MD5 md5 = MD5.Create();
	using FileStream file = File.OpenRead(path);
	return Convert.ToHexString(md5.ComputeHash(file));
}

static void Log(string message, string logPath)
{
	string messageWithTime = DateTime.Now + " " + message;
	Console.WriteLine($"{messageWithTime}");
	File.AppendAllText(logPath, messageWithTime + Environment.NewLine);
}