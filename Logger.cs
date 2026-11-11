class Logger
{
	private readonly string logPath;

	public Logger(string logPath)
	{
		this.logPath = logPath;
	}

	public void Log(string message)
	{
		string messageWithTime = DateTime.Now + " " + message;
		Console.WriteLine(messageWithTime);
		File.AppendAllText(logPath, messageWithTime + Environment.NewLine);
	}
}
