# NMRIH : Server Monitor
- Monitor & Auto Restart the [No more room in hell] server process.
- Made for "nurseryms.ddns.net" Server.
- Windows only.

# Getting Start :
1) Build & Copy executable to server directory which contain server executable.
2) Create or Copy maps.txt, Insert your exist map names and place this file at the same nmrih-server-monitor executable.
3) Quit all your server. (This program will create all server for you.)
4) Start this program and Relax (This program will auto-restart down server's process for you.)
5) Optinal -> Auto Start this program at boot.

# Known issues :
- Make sure you have all your map that list in 'maps.txt', If server process failed to find that missing map -> This program will think Server is still online but can't actually connect to it. (Can solve with using actual RCON protocal to check server status which I will fix in further release.)

# License
- ![MIT](LICENSE)
