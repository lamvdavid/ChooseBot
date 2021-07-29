using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChooseBot.Modules
{
    public class Chooser : ModuleBase<SocketCommandContext>
    {
        readonly Random rnd = new Random();

        [Command("choose")]
        [Alias("c", "pick", "p")]
        [Summary("Choose from a list")]
        public async Task Choose([Remainder] string choices)
        {
            string[] split = choices.Split(',');

            await ReplyAsync(split[rnd.Next(split.Length)]);
        }
    }
}
