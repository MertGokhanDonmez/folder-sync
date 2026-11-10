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