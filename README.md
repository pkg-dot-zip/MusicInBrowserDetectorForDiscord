<p align="center">
  <a href="https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/" rel="noopener">
 <img width=400px height=400px src="docs/mandatory_pepe.png" alt="Project logo"></a>
</p>

<h3 align="center">YT Music RPC Client</h3>

<div align="center">

  [![Stars](https://img.shields.io/github/stars/pkg-dot-zip/MusicInBrowserDetectorForDiscord.svg)](https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/stargazers)
  [![GitHub Issues](https://img.shields.io/github/issues/pkg-dot-zip/MusicInBrowserDetectorForDiscord.svg)](https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/issues)
  [![GitHub Pull Requests](https://img.shields.io/github/issues-pr/pkg-dot-zip/MusicInBrowserDetectorForDiscord.svg)](https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/pulls)
  ![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/pkg-dot-zip/MusicInBrowserDetectorForDiscord/total)

</div>

<p align="center">A simple <a href="https://music.youtube.com/">YouTube Music</a> (and others) Rich Presence client for <a href="https://discord.com/">Discord</a> just like the built-in Spotify one!
</p>

<p align="center">
  <a href="https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/" rel="noopener">
 <img height=400px src="docs/example_crave.png" alt="Project logo"></a>
</p>

## 📝 Table of Contents
- [About](#about)
- [Usage](#usage)
- [Built Using](#built_using)
- [Authors](#authors)

## 🧐 About <a name="about"></a>
An optimized and plug and play console application that automatically retrieves your streaming service from your browser.

### What does it do? 🤔
You know the Rich Presence that you see when your friends listen to [Spotify](https://open.spotify.com/)?
<p align="center">
  <a href="https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/" rel="noopener">
 <img width=640px src="docs/spotify_sonne.png" alt="Project logo"></a>
</p>

This project replicates that but for [YouTube Music](https://music.youtube.com/) and other streaming services! It will automatically retrieve all data from your currently opened browser,
meaning that you only need to open your streaming service in your browser and have Discord open for this to work. We truly live in the golden age! ✨

#### Supported Browsers 🕸
Please feel free to commit a PR to support more browsers, since there are [Selenium](https://www.selenium.dev/selenium/docs/api/dotnet/) webdrivers for other browsers too!

| Browser | Currently Supported | Instructions |
| ------- | --------- | ----- |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/e/e1/Google_Chrome_icon_%28February_2022%29.svg" width="20"/>]() Chrome | ✅ | [Here](docs/_instructions_chrome.md) |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/a/a0/Firefox_logo%2C_2019.svg" width="20"/>]() Firefox | ❌ | - |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/9/98/Microsoft_Edge_logo_%282019%29.svg" width="20"/>]() Edge | ❌ | -|
| [<img src="https://upload.wikimedia.org/wikipedia/en/9/95/Internet_Explorer_9.png" width="20"/>]() Internet Explorer | ❌* | - |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/d/df/Tor_Browser_icon_%28New%29.png" width="20"/>]() Tor | ❌ | - |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/4/49/Opera_2015_icon.svg/640px-Opera_2015_icon.svg.png" width="20"/>]() Opera | ❌ | - |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/4/49/Opera_2015_icon.svg/640px-Opera_2015_icon.svg.png" width="20"/>]() Opera GX | ❌ | - |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/9/9d/Brave_lion_icon.svg" width="20"/>]() Brave | ✅ | Follow [Chrome](docs/_instructions_chrome.md) Instructions |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/Vivaldi_web_browser_logo.svg/2053px-Vivaldi_web_browser_logo.svg.png" width="20"/>]() Vivaldi | ❌ | - |

<sup><sub>* Internet explorer doesn't support any streaming service we support, hence we can not implement support for it.</sub></sup>

#### Supported Streaming Services
Obviously the project's original scope was only YouTube Music, but after widening the projects scope support for other services have also been implemented!

| Streaming Service | Currently Supported |
| ------- | --------- |
| [<img src="https://upload.wikimedia.org/wikipedia/commons/6/6a/Youtube_Music_icon.svg" width="20"/>]() YouTube Music | ✅ |
| [<img src="https://www.svgrepo.com/show/303135/soundcloud-logo.svg" width="20"/>]() SoundCloud | ✅ |

### Why?! 😱
One of my friends listens to music on Spotify the entire day during work, so I can see what music he listens to. However, other people in my friendgroup use YouTube Music, which does not have rich presence support. This screamt for a solution!

#### But that solution already exists!
There are indeed already multiple implementations for this. However, most of them had one of the following issues:
- Some were using way **too much RAM** for what it should do.
- Some were **reading history files** in your browser (I am not kidding! I have actually seen this!!!).
- Some contain an **unnecessary GUI**.
- They were programmed using **complicated unoptimized libraries** to achieve their goal.

### Features 🌟
<p align="center">
  <a href="https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/" rel="noopener">
 <img width=640px src="docs/example_love_is_blind.png" alt="Project logo"></a>
</p>

- Displays the artist and title! 🎙
- Shows the album (if applicable) the song is a part of! 💿
- Artwork is supported and looks better than ever! 😍
- Timestamp of where you are listening! ⌚
- Unique graphics for when you pause your songs! 🎨
- Button to listen along or download this client as well! 🎧
- So performant you will not notice you are running this program! 🌪
- Supports multiple streaming services! 🌎

## 🎈 Usage <a name="usage"></a>
1. Open the Discord app. -> _This does not work in your browser!_
1. Select a browser of your choosing.
1. Go to the docs folder in this project and look for that browser. For example the Chrome instructions [here](docs/_instructions_chrome.md).
1. Follow that one instruction and you're already done. From now on you only have to boot this software and Bob's your uncle!

## ⛏️ Built Using <a name = "built_using"></a>
- [Visual Studio](https://visualstudio.microsoft.com/) - IDE used
- [C#](https://dotnet.microsoft.com/en-us/languages/csharp) - Language used to program in
- [Selenium](https://www.selenium.dev/selenium/docs/api/dotnet/) - Used for interacting with your browser
- [Discord RPC C#](https://github.com/Lachee/discord-rpc-csharp) - C# custom implementation for Discord Rich Presence. For our project we are using a [prerelease](https://github.com/Lachee/discord-rpc-csharp/releases/tag/v1.3.0)

## ✍️ Authors <a name = "authors"></a>
- [@pkg-dot-zip](https://github.com/pkg-dot-zip) - Idea & Initial work.

See also the list of [contributors](https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord/contributors) who participated in this project.
