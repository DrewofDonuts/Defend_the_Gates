using UnityEngine;

namespace DFun.Bookmarks
{
    public static class DiscordHelper
    {
        private const string DiscordInviteLink = "https://discord.gg/J6wpyG5de9";

        public static void Open()
        {
            Application.OpenURL(DiscordInviteLink);
        }
    }
}