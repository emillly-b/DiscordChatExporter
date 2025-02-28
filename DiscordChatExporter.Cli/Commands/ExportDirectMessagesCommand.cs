﻿using System.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using DiscordChatExporter.Cli.Commands.Base;
using DiscordChatExporter.Core.Discord.Data;
using DiscordChatExporter.Core.Utils.Extensions;

namespace DiscordChatExporter.Cli.Commands
{
    [Command("exportdm", Description = "Export all direct message channels.")]
    public class ExportDirectMessagesCommand : ExportCommandBase
    {
        public override async ValueTask ExecuteAsync(IConsole console)
        {
            var cancellationToken = console.RegisterCancellationHandler();

            await console.Output.WriteLineAsync("Fetching channels...");
            var channels = await Discord.GetGuildChannelsAsync(Guild.DirectMessages.Id, cancellationToken);
            var textChannels = channels.Where(c => c.IsTextChannel).ToArray();

            await base.ExecuteAsync(console, textChannels);
        }
    }
}