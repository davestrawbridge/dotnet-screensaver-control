using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Security.Principal;

namespace WindowsScreensaver {

	public class Screensaver {

		/// <summary>
		/// 
		/// Gets or sets the enabled/disabled state of the screensaver
		/// 
		/// when GETTING this flag, SystemParametersInfo has a bug on some versions of Windows (http://support.microsoft.com/kb/318781), so use WMI
		/// when SETTING this flag, WMI is read-only, so use SystemParametersInfo
		/// </summary>
		public bool Enabled {
			get { return (bool)DesktopWmiObject["ScreenSaverActive"]; }
			set {
				uint uiParam = (uint)(value ? 1 : 0);
				SafeNativeMethods.SystemParametersInfo(SafeNativeMethods.SPI_SETSCREENSAVEACTIVE, uiParam, 0, 0);
			}
		}

		/// <summary>
		/// Gets the full path of the current screensaver executable - usually has .scr extension, but not always
		/// </summary>
		public string ExecutablePath {
			get { return (string)DesktopWmiObject["ScreenSaverExecutable"]; }
		}

		/// <summary>
		/// Returns true if the screensaver is currently running
		/// </summary>
		public bool IsRunning {
			get {
				uint pvParam = 0;
				SafeNativeMethods.SystemParametersInfo(SafeNativeMethods.SPI_GETSCREENSAVERRUNNING, 0, ref pvParam, 0);
				return pvParam != 0;
			}
		}

		/// <summary>
		/// Gets or sets the screensaver timeout, in seconds
		/// </summary>
		public int Timeout {
			get {
				uint pvParam = 0;
				SafeNativeMethods.SystemParametersInfo(SafeNativeMethods.SPI_GETSCREENSAVETIMEOUT, 0, ref pvParam, 0);
				return (int) pvParam;
			}
			set {
				uint uiParam = (uint) value;
				SafeNativeMethods.SystemParametersInfo(SafeNativeMethods.SPI_SETSCREENSAVETIMEOUT, uiParam, 0, 0);
			}
		}

		/// <summary>
		/// Gets a list of running screensaver processes.
		/// Note that even when the screensaver isn't running, there can be a process. 
		/// And when it IS running, there can be two processes - only one of which is the actual active screensaver.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Process> GetScreensaverProcesses() {
			string executableName = Path.GetFileName(ExecutablePath);
			List<Process> screensaverProcesses = new List<Process>();
			// NOTE: don't use LINQ so this works in .NET 2.0 as well
			foreach (Process process in Process.GetProcesses())
				if (process.ProcessName.Equals(executableName, StringComparison.InvariantCultureIgnoreCase))
					screensaverProcesses.Add(process);

			return screensaverProcesses;
		}

		/// <summary>
		/// Stops the screensaver, if currently running
		/// </summary>
		public void Stop() {

			foreach (Process process in GetScreensaverProcesses())
				process.CloseMainWindow();
		}

		/// <summary>
		/// Gets the WMI desktop object for the current user.
		/// There will typically be three desktop instances;
		/// one for the system, one for the current user, one .DEFAULT
		/// The current user's desktop is the one we care about.
		/// </summary>
		private ManagementObject DesktopWmiObject {
			get {
				if (desktopWmiObject == null) {

					string wmiPath = string.Format("Win32_Desktop.Name='{0}'", UserName);
					desktopWmiObject = new ManagementObject("root\\cimv2", wmiPath, new ObjectGetOptions());
				}

				return desktopWmiObject;
			}
		}

		private ManagementObject desktopWmiObject;

		/// <summary>
		/// Gets the current user name - this will be something like "MACHINENAME\\username" or "DOMAIN\\username"
		/// </summary>
		private string UserName {
			get {
				if (userName == null) {
					WindowsIdentity identity = WindowsIdentity.GetCurrent();
					userName = identity.Name;
				}
				return userName;
			}
		}

		private string userName;
	}
}
