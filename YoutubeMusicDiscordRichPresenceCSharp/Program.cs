using DiscordRPC;
using YoutubeMusicDiscordRichPresenceCSharp.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{

    public static void Main(string[] args)
    {
        RpcHandler.Initialize();

        RpcHandler.SetPresence(new RichPresence()
        {
            Details = "Example Project",
            State = "csharp example",
            Assets = new Assets()
            {
                LargeImageKey = "image_large",
                LargeImageText = "Lachee's Discord IPC Library",
                SmallImageKey = "image_small"
            }
        });

        Console.ReadKey();

        RpcHandler.Deinitialize();
    }
}