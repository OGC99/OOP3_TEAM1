using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonOOP3
{
    public class Player : IBattle
    {
        public string Name { get; set; }
        public Pokemon ActivePokemon { get; set; }
        public List<Pokemon> Pokemons { get; set; }

        public Player(string name)
        {
            Name = name;
            Pokemons = new List<Pokemon>();
        }

        public void SwitchPokemon(Pokemon newPokemon)
        {
            ActivePokemon = newPokemon;
            Console.WriteLine($"{Name} switched to {newPokemon.Name}!");
        }

        //public void UseMove(Move move, Player opponent)
        //{
        //    Console.WriteLine($"{Name} used {move.Name} on {opponent.Name}'s {opponent.ActivePokemon.Name}!");
        //    // Implement move logic here
        //}

        public void ChoosePokemon()
        {
            // Implement logic for the player to choose a Pokémon during battle
            Console.WriteLine($"{Name}, choose your Pokémon!");
        }

        public void EndBattle(Player winner)
        {
            Console.WriteLine($"The battle has ended. {winner.Name} is the winner!");
        }

        public void StartBattle(Player player1, Player player2)
        {
            Console.WriteLine($"A battle has started between {player1.Name} and {player2.Name}!");
        }
    }
}
