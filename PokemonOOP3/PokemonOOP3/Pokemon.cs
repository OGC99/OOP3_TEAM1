using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonOOP3
{
        public class Pokemon
        {
            public string Name { get; set; }
            public int HP { get; set; }
            public int Attack { get; set; }
            public List<Move> Moves { get; set; }

            public Pokemon()
            {
                Moves = new List<Move>();
            }

            public void TakeDamage(int damage)
            {
                HP -= damage;
                if (HP <= 0)
                {
                    HP = 0;
                    Console.WriteLine($"{Name} fainted!");
                }
                else
                {
                    Console.WriteLine($"{Name} took {damage} damage!");
                }
            }

            public bool IsFainted()
            {
                return HP <= 0;
            }
        }

        public class Move
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public int Power { get; set; }
        }
    
}
