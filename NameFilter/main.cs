using System;
using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using System.Reflection;

namespace Name_Filter
{
    [ApiVersion(1, 16)]
    public class NameFilter : TerrariaPlugin
    {
        string whitelist = "abcdefghijklmnopqrstuvwxyz0123456789 .-'_";
        string[] blacklist = { "redigit", "penis", "retard", "asshole", "douchebag", " butt", "suck", "dick", "hitler", "gay", "nigger", "fuck", "bitch", "yolo", "swag", " ass", "ass ", "cunt", "vagina", "slut", "fag", "homo", "anal", "sperm", "cum", "..", "--", "  ", "''", "__" };
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public override string Author
        {
            get { return "Ancientgods"; }
        }
        public override string Name
        {
            get { return "whitelist"; }
        }

        public override string Description
        {
            get { return "Kicks people with invalid names"; }
        }
        public override void Initialize()
        {
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
        }

        public NameFilter(Main game)
            : base(game)
        {
            Order = -1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
            }
            base.Dispose(disposing);
        }

        private void OnJoin(JoinEventArgs args)
        {
            var player = new TSPlayer(args.Who);
            string name = player.Name.ToLower();

            if (name.Length < 3)
                TShock.Utils.Kick(player, "Minimum name length of 3 characters required!", true, true);

            char[] chars = name.ToCharArray();

            for (int i = 0; i < blacklist.Length; i++)
            {
                if (name.Contains(blacklist[i]))
                {
                    switch (blacklist[i])
                    {
                        case "  ":
                            TShock.Utils.Kick(player, "Only one space between each word is allowed", true, true);
                            return;
                    }
                    TShock.Utils.Kick(player, "Blacklisted word: " + blacklist[i], true, true);
                    return;
                }
            }

            for (int i = 0; i < chars.Length; i++)
            {
                if (!whitelist.Contains(chars[i].ToString()))
                {
                    TShock.Utils.Kick(player, "Unallowed character: " + chars[i], true, true);
                    return;
                }
            }
            TShock.Players[args.Who].SetTeam(4);
        }
    }
}
