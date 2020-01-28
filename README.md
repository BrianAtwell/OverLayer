# OverLayer
Creates an overlay layer on the desktop to provide information to the user. This can be useful for development builds to 
show the user which development builds are being run. Multi build or releases envirnoments can be important to track bugs or business logic changes.

The text displayed can be edited through a IP/TCP connection and sending an XML string.

# Notes to developer
When accessing the Win32 message loop all variables passed via the class must be static.
