﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using DiscordChatExporter.Core.Discord;
using DiscordChatExporter.Core.Discord.Data.Common;
using DiscordChatExporter.Core.Discord.Data.Embeds;
using DiscordChatExporter.Core.Utils.Extensions;
using JsonExtensions.Reading;


namespace DiscordChatExporter.Core.Discord.Data
{

    // https://discord.com/developers/docs/resources/invite
    public partial class Invite
    {
        public string Code { get; set; }
        public Guild Guild { get; set; }
        public Channel Channel { get; set; }

        public static Invite Parse(JsonElement json)
        {
            var code = json.GetProperty("code").GetString();
            var guild = json.GetProperty("guild").Pipe(Guild.Parse);
            var channel = json.GetProperty("channel").Pipe(Channel.Parse);
            return new Invite(code, guild, channel);
        }
        public Invite(string code, Guild guild, Channel channel)
        {
            Code = code;
            Guild = guild;
            Channel = channel;
        }
        public static async ValueTask<List<Invite>> ParseInvites(Message message, DiscordClient context)
        {
            List<Invite>? parsedInvites = new List<Invite>();
            List<string> ids = new List<string>();
            Regex inviteRegex = new Regex(@"(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discordapp\.com\/invite)\/.+[^\s\/]+?");
            var splitContent = message.Content.Split(' ');
            foreach(string part in splitContent)
            {
                MatchCollection matches = inviteRegex.Matches(part);
                for (int current = 0; current < matches.Count; current++)
                {
                    string[] temp = matches[current].Value.Split('/');
                    ids.Add(temp[temp.Length - 1]);
                }
                foreach (string id in ids)
                {
                    var invite = await context.GetInviteAsync(id);
                    parsedInvites.Add(invite);
                }
            }         
            return parsedInvites;
        }
    }

    public partial class Message
    {
        public List<Invite>? Invites { get; set; }
        private bool CheckForInvite(string content)
        {
            Regex inviteRegex = new Regex(@"(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discordapp\.com\/invite)\/(.+[a-z])");
            var splitContent = content.Split(' ');
            var match = inviteRegex.Match(content);
            if (match.Success)
                return true;
            return false;
        }
    }

    public partial class Guild
    {
        public string? Splash { get; set; }

        public string? Banner { get; set; }

        public string? Description { get; set; }

        public object? Icon { get; set; }
    }
}
