dotnet-screensaver-control
==========================

#Control Windows screensavers with managed code.

Windows screensavers are a pain to control - as in start, stop etc.

I realized this when someone asked me how to stop a running screensaver from C#.

All the examples I've seen use P/Invoke and a load of Win32 functions to deal with multiple desktops, SystemParametersInfo etc etc.

The trouble is, most of these techniques work on some versions not others, or only work if the screensaver is set to go back to the login screen.

I thought it would be nice to take advantage of the power of the .net framework instead.
So I've used a bit of WMI and System.Diagnostics classes. It's still a bit messy, but surely better than the alternative.

Hopefully someone else will stumble across this and it will save them a bit of pain.