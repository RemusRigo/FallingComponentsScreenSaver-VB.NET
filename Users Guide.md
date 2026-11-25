# Falling Components ScreenSaver: User's Guide



## Downloads & Runtimes

Download the latest release from the [Releases page](https://github.com/RemusRigo/FallingComponentsScreenSaver-VB.NET/releases).

Each release contains one folder per framework and architecture:

|Folder|Target Framework|Architecture|Required Runtime|
|-|-|-|-|
|`net48 x64`|.NET Framework 4.8|64-bit|Built into Windows 10/11 ([.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) for older systems)|
|`net48 x86`|.NET Framework 4.8|32-bit|Built into Windows 10/11 ([.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) for older systems)|
|`net8.0 x64`|.NET 8.0|64-bit|[.NET 8.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net8.0 x86`|.NET 8.0|32-bit|[.NET 8.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net10.0 x64`|.NET 10.0|64-bit|[.NET 10.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/10.0)|
|`net10.0 x86`|.NET 10.0|32-bit|[.NET 10.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/10.0)|

> **Which one do I need?** If you are not sure, use **`net48 x64`**: it runs on any 64-bit
> Windows 10/11 without installing anything.
> For the .NET 8.0 / 10.0 builds you need the **Desktop Runtime** of that version;
> the plain ".NET Runtime" is not enough.

To check which .NET runtimes are installed, open a command prompt and run:

```
dotnet --list-runtimes
```

Look for a `Microsoft.WindowsDesktop.App 8.x` or `10.x` line.



## Install

1. Download and extract the folder that matches your system (see the table above), e.g. `net48 x64`.
2. Copy the **whole folder** to a permanent location, for example:
   `D:\My Apps\Falling Components ScreenSaver\`
   The folder must contain all of its files; the screensaver won't start if any of them is missing:
   * `net48` folders: `FallingComponentsSS.scr`, `FallingComponentsSS.scr.config`
   * `net8.0` / `net10.0` folders: `FallingComponentsSS.scr`, `FallingComponentsSS.dll`, `FallingComponentsSS.dll.config`,
     `FallingComponentsSS.deps.json`, `FallingComponentsSS.runtimeconfig.json`
3. Right-click `FallingComponentsSS.scr` and choose **Install**.
   The Windows **Screen Saver Settings** dialog opens with *FallingComponentsSS* selected.
4. Set the **Wait** time and click **OK**.

> If Windows blocks the file ("Windows protected your PC"), right-click `FallingComponentsSS.scr` →
> **Properties** → check **Unblock** → **OK**, then try again.



## Test it

* In **Screen Saver Settings**, click **Preview**, or
* right-click `FallingComponentsSS.scr` and choose **Test**.

Computer components (floppy disks, CD-ROMs, cassette tapes, hard disks, CPUs and memory modules)
fall from the top of the screen, spinning as they go.
When one hits the bottom of the screen it breaks into four pieces that bounce off and fly away.
Press any key, click, or move the mouse to exit.

The small monitor in **Screen Saver Settings** shows a live, scaled-down preview.



## Settings

Open **Screen Saver Settings** (Start → type *screen saver*), select *FallingComponentsSS*, and click **Settings...**
(or right-click `FallingComponentsSS.scr` and choose **Configure**).

|Setting|Default|Description|
|-|-|-|
|**Everything Random**|Off|Random colors, all object types mixed, and a random speed for every item. Turns off all the other settings|
|**Random Color**|Off|Every item gets a random color. When off, items use only the colors checked under **Colors**|
|**Colors**|Blue|The colors items are drawn in (Blue, Green, Red), picked at random for each item|
|**Random Objects**|On|Objects are picked at random, see **One / All**. When off, only the objects checked under **Objects** fall|
|**One / All**|All|With **Random Objects**: **One** picks a single object type each time the screensaver starts and only that type falls; **All** mixes every type|
|**Objects**|All|The object types that fall (Floppy Disk, CD-ROM, Cassette Tape, Hard Disk, CPU, Memory)|
|**Speed**|5 (1 - 10)|How fast the items fall: 1 is about 5× slower, 10 is twice as fast (each item still gets a slightly random speed)|

When **Random Color** or **Random Objects** is off, at least one color / object must be checked.

Click **OK** to save the settings; **Cancel**, **Esc** or the **X** button closes the dialog without saving.
They are stored per user in the registry under `HKEY_CURRENT_USER\Software\Remus RIGO\FallingComponentsSS-VB.NET`.
Right-click the title bar → **About...** for version information.



## Multi-monitor

Each monitor gets its own falling items, including monitors with different resolutions or scaling.
With **Random Objects → One**, all monitors show the same object type.



## Uninstall

1. In **Screen Saver Settings**, choose another screensaver (or *None*) and click **OK**.
2. Delete the folder you copied in step 2 of *Install*.
3. Optional: delete the registry key `HKEY_CURRENT_USER\Software\Remus RIGO\FallingComponentsSS-VB.NET`.
