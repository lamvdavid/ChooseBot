using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;

namespace ChooseBot.Modules
{
    public class Roller : ModuleBase<SocketCommandContext>
    {
        Random rnd = new Random();
        private static readonly Regex wSpace = new Regex(@"\s+");

        [Command("flip")]
        [Summary("Flip a coin")]
        public async Task Flip()
        {
            if(rnd.Next(2) == 0) {
                await ReplyAsync("Heads");
            } else {
                await ReplyAsync("Tails");
            }
            
        }

        [Command("roll")]
        [Alias("r")]
        [Summary("Roll a die/dice")]
        public async Task Roll([Remainder] string roll)
        {
            //Roll dice and convert into formula
            string rolledDice = "";

            string trimmed = ReplaceWhitespace(roll, "");

            //Track left and right indices to 
            int left = 0, right;
           
            //Parse dice roll and modifiers
            for(int i = 0; i < trimmed.Length; i++)
            {
                right = i + 1;
                string curChar = trimmed[i].ToString();
                //Roll the dice first and then add the modifier
                if(curChar.Equals("+") || curChar.Equals("-"))
                {
                    //Add modifier
                    rolledDice += curChar;
                    left = i + 1;
                } else if(curChar.Equals("d") || Char.IsNumber(curChar,0)) //Tracks the dice roll
                {
                    //Add dice roll to dice List if last char in the roll
                    if (i + 1 == trimmed.Length || trimmed[i + 1].Equals('+') || trimmed[i + 1].Equals('-'))
                    {
                        rolledDice += RollDice(trimmed[left..right]);
                        left = i + 1;
                    }
                } else
                {
                    await ReplyAsync("Unknown character.");
                    return;
                }
                if (rolledDice.Length > 2000)
                {
                    await ReplyAsync("Message length too long. Reduce # of dice and/or range of dice");
                    return;
                }
            }

            //Sum up dice roll
            int sum = Convert.ToInt32(new DataTable().Compute(rolledDice, null));

            //Display result
            await ReplyAsync($"{sum} = {rolledDice}");
        }

        public string RollDice(string dice)
        {
            //Return if dice is just a modifier
            if (!dice.Contains('d'))
            {
                return dice;
            }
            string[] split = dice.Split('d');
            //Roll die
            if (String.IsNullOrEmpty(split[0]))
            {
                return "(" + rnd.Next(1, Convert.ToInt32(split[1]) + 1).ToString() + ")";
            } else //Roll dice
            {
                string rolledDice = "(";
                for(int i = 0; i < Convert.ToInt32(split[0]); i++)
                {
                    rolledDice += rnd.Next(1, Convert.ToInt32(split[1]) + 1).ToString();
                    if(i + 1 < Convert.ToInt32(split[0]))
                    {
                        rolledDice += "+";
                    }
                }
                return rolledDice + ")";
            }
        }

        public static string ReplaceWhitespace(string input, string replacement)
        {
            return wSpace.Replace(input, replacement);
        }
    }
}
