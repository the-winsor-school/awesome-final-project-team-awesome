﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld
{
    using static TextFormatter;

    public partial class MainClass
    {
        /// <summary>
        /// Enumeration for possible results of doing Combat.
        /// </summary>
        public enum CombatResult
        {
            Lose,
            Win,
            RunAway = -1
        };

        /// <summary>
        /// Commands usable in Combat.
        /// </summary>
        private static List<string> CombatCommands = new List<string>() { "attack", "defend", "use", "run" };

        /// <summary>
        /// Compute a display message for the creature's health.
        /// Does this method belong here?
        /// </summary>
        /// <param name="creature">the Creature you're talking about.</param>
        protected static string hpMessage(Creature creature)
        {
            float percentage =(float)creature.Stats.HPs /(float)creature.Stats.MaxHPs;

            if(percentage >= 1f) return "uninjured";
            else if(percentage > 0.8f) return "barely injured";
            else if(percentage > 0.6f) return "injured";
            else if(percentage > 0.4f) return "wounded";
            else return "badly wounded";
        }

        /// <summary>
        /// Enter combat with a particular creature.
        /// </summary>
        /// <param name="creature">the Creature you're fighting. passed by reference so that it can be modified.</param>
        public static CombatResult DoCombat(ref Creature creature)
        {
            bool inCombat = true;
            while(inCombat)
            {
                bool playerDefending = false;

                // Roll a d20 to determine the creature's action.
                int creatureAction = Dice.Roll20();
                // if the roll is at most 4, defend.
                bool creatureDefending = creatureAction <= 4;
                
                Console.WriteLine("[{0} ({1} / {2})]", player.Name, player.Stats.HPs, player.Stats.MaxHPs);
                Console.Write("[{0} ({1})] << ", creature.Name, hpMessage(creature));
                string command = Console.ReadLine();
                string[] parts = command.Split(' ');

                if(!CombatCommands.Contains(parts[0]))
                {
                    PrintLineWarning("That's not a valid combat command.");
                    continue;
                }

                // parse the combat command!
                // there are many things that can be improved about this.
                switch(parts[0])
                {
                    case "attack":
                        int dmg = player.Stats.CalculateAttack(creature.Stats);
                        if(creatureDefending)
                        {
                            PrintLineWarning("{0} is prepared for your attack...", creature.Name);
                            dmg /= 2;
                        }
                        creature.Stats.HPs -= dmg;
                        PrintLinePositive("You attack and deal {0} dammage!", dmg);
                        break;
                    case "defend":
                        playerDefending = true;
                        PrintLinePositive("You brace for the oncomming attack.");
                        break;
                    case "use":
                        // TODO: Write this!
                        break;
                    case "run":
                        if(Dice.Roll(Dice.Type.Coin) == 1)
                            return CombatResult.RunAway;

                        PrintLineDanger("You tried to run away but you failed!");
                        break;
                    // There is no "default" case here.  Why?
                }

                if(creatureAction <= 4)
                {
                    PrintLineWarning("The creature was defending and takes no action.");
                }
                else if(creatureAction <= 19)
                {
                    int dmg = creature.Stats.CalculateAttack(player.Stats);
                    if(playerDefending)
                        dmg /= 2;

                    player.Stats.HPs -= dmg;

                    PrintLineDanger("{0} attacks you and deals {1} dammage!", creature.Name, dmg);
                }
                else
                {
                    int dmg = 2 * creature.Stats.CalculateAttack(player.Stats);
                    if(playerDefending)
                        dmg /= 2;

                    player.Stats.HPs -= dmg;
                    PrintLineDanger("{0} lands a devastating blow and deals {1} dammage!", creature.Name, dmg);
                }

                if(player.Stats.HPs <= 0) return CombatResult.Lose;
                else if(creature.Stats.HPs <= 0) return CombatResult.Win;
            }

            return CombatResult.Lose;
        }
    }
}
