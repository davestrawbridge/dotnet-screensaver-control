using System.Runtime.InteropServices;

namespace WindowsScreensaver {

	/// <summary>
	/// we don't actually use most of these
	/// </summary>
	internal static class SafeNativeMethods {

		public const uint SPI_GETSCREENSAVETIMEOUT = 0x000E;
		public const uint SPI_SETSCREENSAVETIMEOUT = 0x000F;
		public const uint SPI_GETSCREENSAVEACTIVE = 0x0010;
		public const uint SPI_SETSCREENSAVEACTIVE = 0x0011;
		public const uint SPI_GETSCREENSAVERRUNNING = 0x0072;

		public const uint SPIF_UPDATEINIFILE = 0x01;
		public const uint SPIF_SENDCHANGE = 0x02;
		public const uint SPIF_SENDWININICHANGE = 0x02;

		// use this signature to GET a setting into the pvParam value
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

		// use this signature to SET a setting from the uiParam value
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
	}
}
