using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PokemonOOP3
{
    public partial class PokemonWindow1 : Form
    {
        private List<Pokemon> pokemonList;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public PokemonWindow1()
        {
            InitializeComponent();
            LoadPokemonData();
            PopulatedListBoxSetup();
        }

        private void PokemonWindow1_Load(object sender, EventArgs e)
        {
            DisabledGame();
            ComboBoxChoose.Items.Add("Swap");
            ComboBoxChoose.Items.Add("Attack");
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            SetupGroupBox1.Enabled = true;
            StartButton.Enabled = false;
            ListBoxPlayer1.Enabled = true;
            ListBoxPlayer2.Enabled = true;
        }

        private void DisabledGame()
        {
            ControlGroupBox.Enabled = false;
            BattleGroupBox.Enabled = false;
            SetupGroupBox1.Enabled = false;
        }

        private void LoadPokemonData()
        {
            string json = File.ReadAllText("../../pokemon.json");
            pokemonList = JsonConvert.DeserializeObject<List<Pokemon>>(json);
        }

        private void PopulatedListBoxSetup()
        {
            foreach (Pokemon pokemon in pokemonList)
            {
                string pokemonInfo = $"{pokemon.Name} - HP: {pokemon.HP}, Attack: {pokemon.Attack}";
                ListBoxPlayer1.Items.Add(pokemonInfo);
                ListBoxPlayer2.Items.Add(pokemonInfo);
            }
        }

        private void AddButton1_Click(object sender, EventArgs e)
        {
            if (ListBoxPlayer1.SelectedIndex != -1)
            {
                Pokemon selectedPokemon = pokemonList[ListBoxPlayer1.SelectedIndex];
                if (FinalListBoxPlayer1.Items.Count < 3)
                {
                    FinalListBoxPlayer1.Items.Add(selectedPokemon.Name);
                    if (FinalListBoxPlayer1.Items.Count == 3)
                    {
                        AddButton1.Enabled = false;
                        ListBoxPlayer1.Enabled = false;
                    }
                }
            }
        }

        private void AddButton2_Click(object sender, EventArgs e)
        {
            if (ListBoxPlayer2.SelectedIndex != -1)
            {
                Pokemon selectedPokemon = pokemonList[ListBoxPlayer2.SelectedIndex];
                if (FinalListBoxPlayer2.Items.Count < 3)
                {
                    FinalListBoxPlayer2.Items.Add(selectedPokemon.Name);
                    if (FinalListBoxPlayer2.Items.Count == 3)
                    {
                        AddButton2.Enabled = false;
                        ListBoxPlayer2.Enabled = false;
                    }
                }
            }
        }

        private async void RandomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RandomCheckBox.Checked)
            {
                TextBoxPlayer2.Text = "RandomBot";
                FinalListBoxPlayer2.Items.Clear();
                Random random = new Random();
                for (int i = 0; i < 3; i++)
                {
                    int index = random.Next(0, ListBoxPlayer2.Items.Count);
                    FinalListBoxPlayer2.Items.Add(ListBoxPlayer2.Items[index].ToString().Split('-')[0].Trim());
                    AddButton2_Click(sender, e);
                    await Task.Delay(1000);
                }
                GroupBoxPlayer2.Enabled = false;
            }
        }

        private void ReadyButton_Click(object sender, EventArgs e)
        {
            // Check that player names are not empty or whitespace
            if (string.IsNullOrWhiteSpace(TextBoxPlayer1.Text))
            {
                MessageBox.Show("Please enter Player 1's name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxPlayer2.Text))
            {
                MessageBox.Show("Please enter Player 2's name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check that player names are different
            if (TextBoxPlayer1.Text == TextBoxPlayer2.Text)
            {
                MessageBox.Show("Player 1 and Player 2 cannot have the same name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check that each player has selected three Pokemon
            if (FinalListBoxPlayer1.Items.Count != 3)
            {
                MessageBox.Show("Please select three Pokémon for Player 1.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (FinalListBoxPlayer2.Items.Count != 3)
            {
                MessageBox.Show("Please select three Pokémon for Player 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Rest of the code...
            SetupGroupBox1.Enabled = false;
            BattleGroupBox.Enabled = true;
            ControlGroupBox.Enabled = true;
            MessageBox.Show("Ready for the Battle!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            TextBoxDisplayP1.Text = TextBoxPlayer1.Text;
            TextBoxDisplayP2.Text = TextBoxPlayer2.Text;

            if (FinalListBoxPlayer1.Items.Count > 0)
            {
                TextBoxPokemon1.Text = FinalListBoxPlayer1.Items[0].ToString();
                FinalListBoxPlayer1.Items.RemoveAt(0);
            }

            if (FinalListBoxPlayer2.Items.Count > 0)
            {
                TextBoxPokemon2.Text = FinalListBoxPlayer2.Items[0].ToString();
                FinalListBoxPlayer2.Items.RemoveAt(0);
            }

            TextBoxHPP1.Text = GetPokemonHP(TextBoxPokemon1.Text).ToString();
            TextBoxHPP2.Text = GetPokemonHP(TextBoxPokemon2.Text).ToString();

            player1 = new Player(TextBoxPlayer1.Text);
            player2 = new Player(TextBoxPlayer2.Text);

            currentPlayer = player1;
            TextBoxCurrentPlayer.Text = currentPlayer.Name;
        }
        private int GetPokemonHP(string pokemonName)
        {
            Pokemon pokemon = pokemonList.FirstOrDefault(p => p.Name.StartsWith(pokemonName));
            if (pokemon != null)
            {
                return pokemon.HP;
            }
            else
            {
                return 0;
            }
        }

        private void ComboBoxChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            string action = ComboBoxChoose.SelectedItem.ToString();

            if (action == "Swap")
            {
                ShowPokemonsToSwap();
            }
            else if (action == "Attack")
            {
                ShowAvailableAttacks();
            }
        }

        private void ShowPokemonsToSwap()
        {
            // Clear the ListBoxChoose
            ListBoxChoose.Items.Clear();

            // Get the list of Pokémon of the current player from the corresponding final ListBox
            ListBox finalListBoxPlayer = currentPlayer == player1 ? FinalListBoxPlayer1 : FinalListBoxPlayer2;

            // Get the current player's Pokémon from the corresponding final ListBox
            foreach (object item in finalListBoxPlayer.Items)
            {
                ListBoxChoose.Items.Add(item.ToString());
            }
        }

        private void ShowAvailableAttacks()
        {
            // Get the name of the current Pokémon
            string currentPokemonName = currentPlayer == player1 ? TextBoxPokemon1.Text : TextBoxPokemon2.Text;

            // Find the current Pokémon in the JSON data
            Pokemon currentPokemon = pokemonList.FirstOrDefault(p => p.Name == currentPokemonName);

            if (currentPokemon != null)
            {
                // Clear the ListBox of moves
                ListBoxChoose.Items.Clear();

                // Show available attacks for the current Pokémon in the moves ListBox
                foreach (var move in currentPokemon.Moves)
                {
                    ListBoxChoose.Items.Add($"{move.Name} (Type: {move.Type}, Power: {move.Power})");
                }
            }
            else
            {
                MessageBox.Show("The current Pokémon was not found in the data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            string action = ComboBoxChoose.SelectedItem.ToString();

            if (action == "Swap")
            {
                // Perform Pokémon swap
                PerformSwap();
            }
            else if (action == "Attack")
            {
                // Perform attack
                PerformAttack();
            }
            ChangePlayer();
            ListBoxChoose.Items.Clear();
        }

        private void PerformSwap()
        {
            // Check if an item is selected in ListBoxChoose
            if (ListBoxChoose.SelectedItem != null)
            {
                // Get the name of the selected Pokémon to swap
                string selectedPokemon = ListBoxChoose.SelectedItem.ToString();

                // Update the current player's Pokémon with the selected one to swap
                if (currentPlayer == player1)
                {
                    TextBoxPokemon1.Text = selectedPokemon;
                }
                else
                {
                    TextBoxPokemon2.Text = selectedPokemon;
                }

                // Show available Pokémon to swap
                ShowPokemonsToSwap();
            }
            else
            {
                MessageBox.Show("Please select a Pokémon to swap.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PerformAttack()
        {
            // Check if an item is selected in ListBoxChoose
            if (ListBoxChoose.SelectedItem != null)
            {
                // Get the name of the current player's Pokémon and the opponent's Pokémon
                string attackingPokemonName = currentPlayer == player1 ? TextBoxPokemon1.Text : TextBoxPokemon2.Text;
                string opponentPokemonName = currentPlayer == player1 ? TextBoxPokemon2.Text : TextBoxPokemon1.Text;

                // Find the Pokémon in the JSON data
                Pokemon attackingPokemon = pokemonList.FirstOrDefault(p => p.Name == attackingPokemonName);
                Pokemon opponentPokemon = pokemonList.FirstOrDefault(p => p.Name == opponentPokemonName);

                // Check if the Pokémon were found
                if (attackingPokemon != null && opponentPokemon != null)
                {
                    // Get the selected move for the attack
                    string selectedMove = ListBoxChoose.SelectedItem.ToString();
                    string moveName = selectedMove.Split('(')[0].Trim();

                    // Find the move in the attacking Pokémon's moves
                    Move move = attackingPokemon.Moves.FirstOrDefault(m => m.Name == moveName);

                    // Check if the move was found
                    if (move != null)
                    {
                        // Perform the attack and update the GUI
                        int power = move.Power;
                        opponentPokemon.TakeDamage(power);

                        // Update the GUI with the new HP values
                        if (currentPlayer == player1)
                        {
                            TextBoxHPP2.Text = opponentPokemon.HP.ToString();
                        }
                        else
                        {
                            TextBoxHPP1.Text = opponentPokemon.HP.ToString();
                        }

                        // Check if the opponent's Pokémon has fainted
                        if (opponentPokemon.IsFainted())
                        {
                            MessageBox.Show($"The Pokémon {opponentPokemon.Name} has fainted!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // If the Pokémon has fainted, automatically select the opponent's next Pokémon if they have one
                            SelectNextOpponentPokemon();
                        }
                    }
                    else
                    {
                        MessageBox.Show("The selected move was not found in the moves of the attacking Pokémon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Pokémon were not found in the data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a move to attack.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectNextOpponentPokemon()
        {
            if (currentPlayer == player1)
            {
                if (FinalListBoxPlayer2.Items.Count > 0)
                {
                    TextBoxPokemon2.Text = FinalListBoxPlayer2.Items[0].ToString();
                    FinalListBoxPlayer2.Items.RemoveAt(0);
                }
                else
                {
                    MessageBox.Show("Player 2 has no more Pokémon! Player 1 wins!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (FinalListBoxPlayer1.Items.Count > 0)
                {
                    TextBoxPokemon1.Text = FinalListBoxPlayer1.Items[0].ToString();
                    FinalListBoxPlayer1.Items.RemoveAt(0);
                }
                else
                {
                    MessageBox.Show("Player 1 has no more Pokémon! Player 2 wins!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ChangePlayer()
        {
            currentPlayer = (currentPlayer == player1) ? player2 : player1;
            TextBoxCurrentPlayer.Text = currentPlayer.Name;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            // Clear ListBox
            //ListBoxPlayer1.Items.Clear();
            //ListBoxPlayer2.Items.Clear();
            FinalListBoxPlayer1.Items.Clear();
            FinalListBoxPlayer2.Items.Clear();
            ListBoxChoose.Items.Clear();
            StartButton.Enabled = true;


            // Clear TextBox
            TextBoxPlayer1.Clear();
            TextBoxPlayer2.Clear();
            TextBoxDisplayP1.Clear();
            TextBoxDisplayP2.Clear();
            TextBoxPokemon1.Clear();
            TextBoxPokemon2.Clear();
            TextBoxHPP1.Clear();
            TextBoxHPP2.Clear();
            TextBoxCurrentPlayer.Clear();

            // Enable SetupGroupBox and disable BattleGroupBox and ControlGroupBox

            BattleGroupBox.Enabled = false;
            ControlGroupBox.Enabled = false;
            SetupGroupBox1.Enabled = false;
            DisabledGame();

            // Show informational message
            MessageBox.Show("The game has been restarted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void HelpButton_Click(object sender, EventArgs e)
        {
            StringBuilder helpMessage = new StringBuilder();

            // Add information about the game
            helpMessage.AppendLine("Welcome to the Pokemon Battle Game!");
            helpMessage.AppendLine("===================================");

            // Explain the objective of the game
            helpMessage.AppendLine("Objective:");
            helpMessage.AppendLine("The goal of the game is to defeat all of your opponent's Pokemon. This is achieved by strategically selecting your moves and Pokemon to outsmart your opponent.");

            // Add a separator
            helpMessage.AppendLine();

            // Explain the game controls
            helpMessage.AppendLine("Controls:");
            helpMessage.AppendLine("---------");
            helpMessage.AppendLine("- Select three Pokemon for each player to battle with. These Pokemon will be your team throughout the game.");
            helpMessage.AppendLine("- Choose 'Swap' to switch your current Pokemon during battle. This allows you to adapt to your opponent's choices and strategies.");
            helpMessage.AppendLine("- Choose 'Attack' to execute an attack against your opponent's Pokemon. Each Pokemon has a set of moves with different types (such as Fire, Water, Grass, etc.) and powers.");
            helpMessage.AppendLine("- Keep track of your Pokemon's HP (Hit Points) to know when it's time to switch. If a Pokemon's HP reaches zero, it faints and can no longer participate in battle.");


            // Show the message box with the collected information
            MessageBox.Show(helpMessage.ToString(), "Game Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
