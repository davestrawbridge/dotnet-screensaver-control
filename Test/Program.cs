using System;
using System.Diagnostics;
using System.Threading;
using WindowsScreensaver;

namespace Test {
	/// <summary>
	/// Not a 'proper' test, just something to show how it works
	/// </summary>
	class Program {
		static void Main(string[] args) {

			Screensaver screensaver = new Screensaver();

			// remember current settings;
			int oldTimeout = screensaver.Timeout;
			bool oldEnabled = screensaver.Enabled;
			Console.WriteLine("Existing settings: timeout={0} seconds, enabled={1}", oldTimeout, oldEnabled);

			// set screensaver timeout to something short
			screensaver.Timeout = 30;
			screensaver.Enabled = true;

			Console.WriteLine("New settings: timeout={0} seconds, enabled={1}", screensaver.Timeout, screensaver.Enabled);

			Console.WriteLine("Screensaver is running: {0}", screensaver.IsRunning);
			Console.WriteLine("Screensaver processes:");
			foreach (Process p in screensaver.GetScreensaverProcesses())
				Console.WriteLine("\t{0} [{1}]", p.ProcessName, p.Id);

			Console.WriteLine("Waiting 1 minute for screensaver to start...");

			Thread.Sleep(60 * 1000);

			Console.WriteLine("1 minute delay is complete");

			Console.WriteLine("Screensaver is running: {0}", screensaver.IsRunning);
			Console.WriteLine("Screensaver processes:");
			foreach (Process p in screensaver.GetScreensaverProcesses())
				Console.WriteLine("\t{0} [{1}]", p.ProcessName, p.Id);

			Console.WriteLine("Stopping screensaver");
			screensaver.Stop();
			Console.WriteLine("Screensaver stopped");

			Console.WriteLine("Screensaver is running: {0}", screensaver.IsRunning);
			Console.WriteLine("Screensaver processes:");
			foreach (Process p in screensaver.GetScreensaverProcesses())
				Console.WriteLine("\t{0} [{1}]", p.ProcessName, p.Id);

			//restore old settings
			screensaver.Timeout = oldTimeout;
			screensaver.Enabled = oldEnabled;
			Console.WriteLine("Restored original settings: timeout={0} seconds, enabled={1}", screensaver.Timeout, screensaver.Enabled);

			Console.WriteLine("Press <enter> to finish.");
			Console.ReadLine();	// so we don't lose the output
		}
	}
}
