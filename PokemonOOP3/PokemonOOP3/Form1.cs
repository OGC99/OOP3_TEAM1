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
using Newtonsoft.Json; // Asegúrate de incluir este using

namespace PokemonOOP3
{
    public partial class PokemonWindow1 : Form
    {
        private List<Pokemon> pokemonList;

        public PokemonWindow1()
        {
            InitializeComponent();
            LoadPokemonData();
            PopulatedListBoxSetup();
        }
        private void PokemonWindow1_Load(object sender, EventArgs e)
        {
            DisabledGame();
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            SetupGroupBox1.Enabled = true;

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
            // Agregar cada Pokemon al ListBox
            foreach (Pokemon pokemon in pokemonList)
            {
                // Formatear el texto para cada Pokemon
                string pokemonInfo = $"{pokemon.Name} - HP: {pokemon.HP}, Attack: {pokemon.Attack}";

                // Agregar el Pokemon al ListBoxPlayer1
                ListBoxPlayer1.Items.Add(pokemonInfo);

                // Agregar el Pokemon al ListBoxPlayer2
                ListBoxPlayer2.Items.Add(pokemonInfo);
            }
        }

        private void AddButton1_Click(object sender, EventArgs e)
        {    // Verificar si se ha seleccionado un ítem en ListBoxPlayer1
            if (ListBoxPlayer1.SelectedIndex != -1)
            {
                // Obtener el Pokémon seleccionado
                Pokemon selectedPokemon = pokemonList[ListBoxPlayer1.SelectedIndex];

                // Verificar si ya hay tres Pokémon en FinalListBoxPlayer1
                if (FinalListBoxPlayer1.Items.Count < 3)
                {
                    // Agregar el Pokémon seleccionado a FinalListBoxPlayer1
                    FinalListBoxPlayer1.Items.Add(selectedPokemon.Name);

                    // Deshabilitar el botón AddButton1 si ya hay tres Pokémon en FinalListBoxPlayer1
                    if (FinalListBoxPlayer1.Items.Count == 3)
                    {
                        AddButton1.Enabled = false;
                        ListBoxPlayer1.Enabled = false;
                    }
                }

                // Deseleccionar el ítem en ListBoxPlayer1
                //ListBoxPlayer1.ClearSelected();
            }

        }

        private void AddButton2_Click(object sender, EventArgs e)
        {
            if (ListBoxPlayer2.SelectedIndex != -1)
            {
                Pokemon selectedPokemon = pokemonList[ListBoxPlayer2.SelectedIndex];
                // Verificar si ya hay tres elementos en FinalListBoxPlayer2
                if (FinalListBoxPlayer2.Items.Count < 3)
                {
                    // Agregar el Pokémon seleccionado de ListBoxPlayer2 a FinalListBoxPlayer2
                    FinalListBoxPlayer2.Items.Add(selectedPokemon.Name);

                    // Deshabilitar el botón AddButton2 si se han seleccionado tres Pokémon
                    if (FinalListBoxPlayer2.Items.Count == 3)
                    {
                        AddButton2.Enabled = false;
                        ListBoxPlayer2.Enabled = false;
                    }
                }
                // Deseleccionar el ítem en ListBoxPlayer1
                //ListBoxPlayer1.ClearSelected();
            }
        }

        private async void RandomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RandomCheckBox.Checked)
            {
                TextBoxPlayer2.Text = "RandomBot";
                // Limpiar FinalListBoxPlayer2
                FinalListBoxPlayer2.Items.Clear();

                // Seleccionar aleatoriamente tres Pokémon en ListBoxPlayer2 y moverlos a FinalListBoxPlayer2
                Random random = new Random();
                for (int i = 0; i < 3; i++)
                {
                    int index = random.Next(0, ListBoxPlayer2.Items.Count);
                    //FinalListBoxPlayer2.Items.Add(ListBoxPlayer2.Items[index]);
                    FinalListBoxPlayer2.Items.Add(ListBoxPlayer2.Items[index].ToString().Split('-')[0].Trim());
                    AddButton2_Click(sender, e); // Llamar al evento AddButton2_Click para simular el clic en el botón "Add"
                    await Task.Delay(1000); // Esperar 1 segundo entre cada selección (simulación de acción robot)
                }

                // Deshabilitar RandomCheckBox después de seleccionar tres Pokémon aleatorios
                GroupBoxPlayer2.Enabled = false;
            }
        }

        private void ReadyButton_Click(object sender, EventArgs e)
        {
            // Validate that player names are entered
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

            // Validate that three Pokémon are selected for each player
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

            // Block the SetupGroupBox
            SetupGroupBox1.Enabled = false;
            BattleGroupBox.Enabled = true;
            ControlGroupBox.Enabled = true;
            MessageBox.Show("Ready for the Battle!","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Print player names
            TextBoxDisplayP1.Text = TextBoxPlayer1.Text;
            TextBoxDisplayP2.Text = TextBoxPlayer2.Text;

            // Print the first Pokémon of each player and remove them from the lists
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

            // Set HP of the selected Pokémon
            TextBoxHPP1.Text = GetPokemonHP(TextBoxPokemon1.Text).ToString();
            TextBoxHPP2.Text = GetPokemonHP(TextBoxPokemon2.Text).ToString();

            Player player1 = new Player() 

                player1.Name = TextBoxPokemon1.Text;
        }

        private int GetPokemonHP(string pokemonName)
        {
            // Buscar el Pokémon en la lista por su nombre
            Pokemon pokemon = pokemonList.FirstOrDefault(p => p.Name.StartsWith(pokemonName));
            if (pokemon != null)
            {
                // Devolver el valor de HP del Pokémon encontrado
                return pokemon.HP;
            }
            else
            {
                // Si no se encuentra el Pokémon, devolver 0 o manejar el error según corresponda
                return 0;
            }
        }




    }
}
