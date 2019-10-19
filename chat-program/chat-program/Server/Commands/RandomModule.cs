using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    [Name("Random Module")]
    public class RandomModule : CommandBase
    {

        [Name("dice")]
        public void RollADice()
        {
            var val = Common.RND.Next(1, 7);
            BroadCast($"{Context.User.Name}({Context.User.Id}) rolled a {val}", System.Drawing.Color.Purple);
        }

        [Name("dice")]
        [Priority(2)]
        public void RollANSidedDice(int sides)
        {
            var val = Common.RND.Next(1, sides + 1);
            BroadCast($"{Context.User.Name}({Context.User.Id}) rolled a {val} (d{sides})", System.Drawing.Color.Purple);
        }

        [Name("dice")]
        [Priority(1)]
        public void RollDnDRice(string text)
        {
            var split = text.Split('d').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if(split.Count() == 1)
            {
                if(int.TryParse(split[0], out var sides))
                {
                    RollANSidedDice(sides);
                } else
                {
                    Reply($"Invalid input: expected 'd[number]', eg 'd6' 'd8' etc.", System.Drawing.Color.Red);
                }
            } else if (split.Count() == 2)
            {
                if(int.TryParse(split[0], out var dices) && int.TryParse(split[1], out var sides))
                {
                    int[] rolls = new int[dices];
                    for (int i = 0; i < dices; i++)
                        rolls[i] = Common.RND.Next(1, sides + 1);
                    BroadCast($"{Context.User.Name}({Context.User.Id}) rolled [{string.Join(", ", rolls)}] sum {rolls.Sum()} of {dices}d{sides}", System.Drawing.Color.Purple);
                }
            }
        }

        [Name("coin")]
        public void DoAFlip()
        {
            var dble = Common.RND.NextDouble();
            if(dble > 0.5)
                BroadCast($"{Context.User.Name}({Context.User.Id}) flipped coin on heads", System.Drawing.Color.Purple);
            else if (dble < 0.5)
                BroadCast($"{Context.User.Name}({Context.User.Id}) flipped coin on tails", System.Drawing.Color.Purple);
            else
                BroadCast($"{Context.User.Name}({Context.User.Id}) flipped coin on its side", System.Drawing.Color.Purple);
        }
    }
}
