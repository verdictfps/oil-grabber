dotnet build

Remove-Item "G:\SteamLibrary\steamapps\common\SULFUR\BepInEx\plugins\oilgrabber\OilGrabber.deps.json"
Remove-Item "G:\SteamLibrary\steamapps\common\SULFUR\BepInEx\plugins\oilgrabber\OilGrabber.dll"
Remove-Item "G:\SteamLibrary\steamapps\common\SULFUR\BepInEx\plugins\oilgrabber\OilGrabber.pdb"

Copy-Item "C:\Users\verdi\source\repos\oil-grabber-main\bin\Debug\netstandard2.1\OilGrabber.deps.json" "G:\SteamLibrary\steamapps\common\SULFUR\BepInEx\plugins\oilgrabber"
Copy-Item "C:\Users\verdi\source\repos\oil-grabber-main\bin\Debug\netstandard2.1\OilGrabber.dll" "G:\SteamLibrary\steamapps\common\SULFUR\BepInEx\plugins\oilgrabber"
Copy-Item "C:\Users\verdi\source\repos\oil-grabber-main\bin\Debug\netstandard2.1\OilGrabber.pdb" "G:\SteamLibrary\steamapps\common\SULFUR\BepInEx\plugins\oilgrabber"