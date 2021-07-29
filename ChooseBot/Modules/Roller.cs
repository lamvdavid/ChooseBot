using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace ChooseBot.Modules
{
    public class Roller : ModuleBase<SocketCommandContext>
    {
        Random rnd = new Random();

        [Command("flip")]
        [Summary("Flip a coin")]
        public async Task Flip()
        {
            if(rnd.Next(2) == 0)
            {
                await ReplyAsync("Heads");
            } else
            {
                await ReplyAsync("Tails");
            }
            
        }
    }
}
