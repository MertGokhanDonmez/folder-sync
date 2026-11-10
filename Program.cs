if(args.Length != 4)
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
IEnumerable<string> replicaFiles = Directory.EnumerateFiles(replicaPath, "*", SearchOption.AllDirectories);

foreach (var item in sourceFiles)
{
	string replicaFilesPath = Path.Combine(replicaPath, Path.GetRelativePath(sourcePath, item));
	string? replicaDirectoryName = Path.GetDirectoryName(replicaFilesPath);
	if (replicaDirectoryName != null)
	{
		Directory.CreateDirectory(replicaDirectoryName);
	}
	File.Copy(item, replicaFilesPath, true);
}

foreach (var item in replicaFiles)
{
	string sourceFilePath = Path.Combine(sourcePath, Path.GetRelativePath(replicaPath, item));
	if (!File.Exists(sourceFilePath))
	{
		File.Delete(item);
	}
}