using Amazon.Lambda.Core;

namespace TrainCheck
{
	public static class App
	{
		public static ILambdaLogger Logger { private get; set; }

		public static void Log(string message)
		{
			Logger.LogLine(message);
		}
	}
}
