// InventoryManager.cs
using System;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Text.Json;

public class Pokemon
{
    public string NomPkm { get; set; }
    public int VieMax { get; set; }
    public int VieActuelle { get; set; }
    public string ID { get; set; }
    public int XP { get; set; }
    public int Niveau { get; set; }
    public string Atk1 { get; set; }
    public int ForceAtk1 { get; set; }
    public int UsesLeftAtk1 { get; set; }
    public string Atk2 { get; set; }
    public int ForceAtk2 { get; set; }
    public int UsesLeftAtk2 { get; set; }

}

class InventoryManager
{




    public static Pokemon SearchPKM()
    {
        string ListePkm = "../../../asset/ListePkm.txt";
        string[] lines = File.ReadAllLines(ListePkm);

        Console.WriteLine("Choisissez un Starter à ajouter entre 1 et 5");
        string IDpkm;
        while (true)
        {
            IDpkm = Console.ReadLine();

            // Chack si la valeur donnée est entre 0 et 5
            if (int.TryParse(IDpkm, out int chosenID) && chosenID >= 0 && chosenID <= 5)
            {
                break;
            }
            else
            {
                Console.WriteLine("Choix invalide, Choisir entre 1 and 5.");
            }
        }
        IDpkm = "#" + IDpkm;

        // Search for the Pokémon in the file
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == IDpkm)
            {
                Pokemon pokemon = new Pokemon();

                // Retrieve Pokémon information
                pokemon.NomPkm = lines[i - 2];
                pokemon.VieMax = int.TryParse(lines[i - 1], out int vieMax) ? vieMax : 0;
                pokemon.VieActuelle = int.TryParse(lines[i - 1], out int VieActuelle) ? vieMax : 0;
                pokemon.ID = IDpkm;
                pokemon.XP = int.TryParse(lines[i + 1], out int xp) ? xp : 0;
                pokemon.Niveau = int.TryParse(lines[i + 2], out int niveau) ? niveau : 0;
                pokemon.Atk1 = lines[i + 3];
                pokemon.ForceAtk1 = int.TryParse(lines[i + 4], out int forceAtk1) ? forceAtk1 : 0;
                pokemon.UsesLeftAtk1 = int.TryParse(lines[i + 5], out int usesLeftAtk1) ? usesLeftAtk1 : 0;
                pokemon.Atk2 = i + 6 < lines.Length && !string.IsNullOrWhiteSpace(lines[i + 6]) ? lines[i + 6] : "";
                pokemon.ForceAtk2 = pokemon.Atk2 != "" ? int.TryParse(lines[i + 7], out int forceAtk2) ? forceAtk2 : 0 : 0;
                pokemon.UsesLeftAtk2 = pokemon.Atk2 != "" ? int.TryParse(lines[i + 8], out int usesLeftAtk2) ? usesLeftAtk2 : 0 : 0;
                InvTempo(pokemon);
                return pokemon;
            }
        }

        // Return null if Pokémon not found
        return null;
    }

    public static void TempToSave ()
    {
        // Copy the content of InvJoueurTemp.txt to Save.txt
        string invJoueurTempPath = "../../../asset/InvJoueurTemp.txt";
        string saveFilePath = "../../../asset/Save.txt";

        try
        {
            string[] invJoueurTempContent = File.ReadAllLines(invJoueurTempPath);
            File.WriteAllLines(saveFilePath, invJoueurTempContent);
        }
        catch (IOException e)
        {
            Console.WriteLine("Erreur à la copie des données : " + e.Message);
        }
    }
    public static void InvTempo(Pokemon pokemon)
    {
        // Step 3: Serialize the class instance to JSON
        string jsonString = JsonSerializer.Serialize(pokemon);

        // Step 4: Write the JSON string to a text file
        string filePath = "../../../asset/InvJoueurTemp.txt";
        File.WriteAllText(filePath, jsonString);


    }

    public static void LevelUp(Pokemon pokemon)
    {
        // Implement your leveling up logic here
        // For example, you can define a threshold XP for leveling up
        int xpThreshold = 100;

        if (pokemon.XP >= xpThreshold)
        {
            // Level up the Pokémon
            pokemon.Niveau++;
            Console.WriteLine($"{pokemon.NomPkm} leveled up to Level {pokemon.Niveau}!");

            // Reset XP to 0 after leveling up
            pokemon.XP = 0;
        }
    }
    public static void SaveLevelAndXP(Pokemon pokemon)
    {
        // Save the level and XP to InvJoueurTemp.txt
        string jsonString = JsonSerializer.Serialize(pokemon);
        string filePath = "../../../asset/InvJoueurTemp.txt";
        File.WriteAllText(filePath, jsonString);
/*        Console.WriteLine($"Level and XP saved to {filePath}");*/
    }
}



    /*public static void DisplayInventory(string[] inventory)
    {
        ConsoleKeyInfo key;
        int pointerPos = 1;

        do
        {
            Console.Clear();
            // Display the inventory with the cursor highlighting the selected slot
            for (int i = 0; i < inventory.Length; i++)
            {
                if (i == pointerPos)
                {
                    // Change color for the four lines below the selected slot
                    for (int j = 0; j <= 3 && i + j < inventory.Length; j++)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(inventory[i + j]);
                        Console.ResetColor();
                    }

                    // Skip the next four lines
                    i += 3;
                }
                else
                {
                    Console.WriteLine(inventory[i]);
                }
            }

            key = Console.ReadKey(true); // Wait for user input

            // Update the cursor position based on user input
            switch (key.Key)
            {
                case ConsoleKey.Z:
                    pointerPos = Math.Max(1, pointerPos - 4); // Move the cursor up
                    break;
                case ConsoleKey.S:
                    pointerPos = Math.Min(inventory.Length - 5, pointerPos + 4); // Move the cursor down
                    break;
            }
        } while (key.Key != ConsoleKey.I); // Exit the loop if the I key is pressed
        Console.Clear();
    }*/
    /*public static void SavePokemonToInventory(Pokemon pokemon)
    {
        string InvModifPath = "../../../asset/InvModif.txt";
        string[] InvModifLines = File.ReadAllLines(InvModifPath);

        // Find the first occurrence of '#' and ':' and replace them with Pokémon's name and maximum life
        for (int i = 0; i < InvModifLines.Length; i++)
        {
            if (InvModifLines[i].Contains("#"))
            {
                InvModifLines[i] = InvModifLines[i].Replace("#", pokemon.NomPkm);
                break;
            }
        }

        for (int i = 0; i < InvModifLines.Length; i++)
        {
            if (InvModifLines[i].Contains(":"))
            {
                InvModifLines[i] = InvModifLines[i].Replace(":", pokemon.VieMax.ToString());
                break;
            }
        }

        // Save the modified lines back to the "InvModif" file
        File.WriteAllLines(InvModifPath, InvModifLines);
    }

    public static void DisplayInventory()
    {
        // ... (existing code)

        // Deserialize the inventory and display it
        Console.WriteLine("Inventory from InvModif:");
        foreach (string line in File.ReadAllLines("../../../asset/InvModif.txt"))
        {
            Console.WriteLine(line);
        }
    }*/


