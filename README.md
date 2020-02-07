# OverLayer
Creates an overlay layer on the desktop to provide information to the user. This can be useful for development builds to 
show the user which development builds are being run. Multi build or releases envirnoments can be important to track bugs or business logic changes.

The text displayed can be edited through a IP/TCP connection and sending an XML string.

# Building
To build, set build configuration to 'release' this will disable debugging.
The OverLayerCSharp project contains the main project.
The OverLayer project contains test code written in C/C++.
The ClientNetworkTest is an example client program written in C Sharp.

## Debug mode
When debugging is enabled the window will not be topmost and will still display in the taskbar. This will allow you to easily close it.

Also you will be able to disable the transparency through the DebugState class.

## Release mode

# Notes to developer
When accessing the Win32 message loop all variables passed via the class must be static.
