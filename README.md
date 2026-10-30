# Gwent Access

A screen reader mod for **Gwent: The Witcher Card Game** (Steam, Windows) that makes the game playable with **NVDA**.

It reads the menus, cards, popups and the battle board, and adds keyboard commands for playing matches, building decks and using the shop. It was built from scratch by Claude (Anthropic) together with **smoovgunner**, a blind player who did all of the testing by ear.

## Download and install

Players: grab the ready-made package (BepInEx included) from the release posted on the [AudioGames.net forum](https://forum.audiogames.net/) or this repository's Releases page. Unzip it, run `Install.bat`, and read `README.txt` for setup and every hotkey.

You will need sighted help once to log in to GOG through Gwent's launcher. After that, the **Gwent Accessible** desktop shortcut skips the launcher.

## What's in this repository

- `src/`: the mod's C# source (a BepInEx 6 IL2CPP plugin).
- `package/`: the installer scripts, the launcher-skip script and the player readme that ship in the release zip.

## Building

Requirements:
- .NET SDK 6 or newer.
- Gwent installed with **BepInEx 6.0.0-be.790 (IL2CPP)**, started once so BepInEx generates `BepInEx\interop`.

```
cd src
dotnet build -c Release
```

If Gwent isn't in the default Steam folder, add `-p:GwentDir="D:\path\to\GWENT The Witcher Card Game"`.

Copy `bin\Release\net6.0\GwentAccess.dll` and NVDA's `nvdaControllerClient.dll` (64-bit) into `BepInEx\plugins\GwentAccess\`.

## How it works (short tour)

| File | What it does |
|---|---|
| `Plugin.cs` | Entry point; applies each Harmony patch separately so one failure can't disable the mod. |
| `Speech.cs` | Talks to NVDA through `nvdaControllerClient.dll`. |
| `Hotkeys.cs` | Reads the keyboard on a background thread; work runs on the game thread through `MainThread.cs`. |
| `MenuSpeech.cs` | Hooks `CrimsonInputLayer.set_SelectedControl` and describes whatever the game selects. |
| `Navigator.cs` | F2/F3 object navigation, sections (Shift+F2/F3), clicking, settings and deck builder lists, shop buying. |
| `ScreenSpeech.cs`, `PopupSpeech.cs`, `KeyHints.cs` | Screen names, popups with their keys, and key hints. |
| `BattleSpeech.cs`, `HandNav.cs` | Match commentary, hand/board reading, mulligan, pass, end turn, turn timer, forfeit guard. |
| `DeckBuilder.cs` | Add/remove cards, deck stats, rename, duplicate, search, save and exit. |
| `CardText.cs`, `RewardBookText.cs`, `SocialNames.cs` | Turning cards, reward nodes and icons into words. |

Logs go to `BepInEx\LogOutput.log` and record every spoken line and hotkey, which makes bug reports easy to trace.

## Contributing

Suggestions and pull requests are welcome. Please describe what you tested and on which screen, and include a `LogOutput.log` excerpt for bug fixes. Changes are reviewed before they're merged.

## License

MIT. See [LICENSE](LICENSE). Gwent is a trademark of CD PROJEKT RED; this is an unofficial fan mod. BepInEx and NVDA's controller client are separate projects under their own licenses (LGPL 2.1).
