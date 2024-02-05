// InventoryManager.cs
using System;
using System.Reflection.Emit;

class InventoryManager
{
    public static void DisplayInventory(string[] inventory)
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
    }


    public static string SearchPKM (string NomPkm, int VieMax, int ID, int XP, int Niveau, string Atk1, int ForceAtk1, int UsesLeftAtk1, string Atk2, int ForceAtk2, int UsesLeftAtk2)
    {

        string ListePkm = "../../../asset/ListePkm.txt" ;
        string[] lines = File.ReadAllLines(ListePkm);

        Console.Write("Choisissez un pkm a ajoter entre 1 et 4\n");
        string IDpkm = Console.ReadLine();
        while (IDpkm.Length < 1 )
        {
            Console.Write("\nChoisissez un pkm a ajoter entre 1 et 4");
            IDpkm = Console.ReadLine();
        }
        IDpkm = "#" + IDpkm;
        Console.WriteLine("\n");

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == IDpkm)
            {
                // Ensure there are enough lines below and above
                if (i - 2 >= 0 && i + 5 < lines.Length)
                {
                    NomPkm = lines[i - 2];
                    if (!int.TryParse(lines[i - 1], out VieMax)) VieMax = 0;
                    if (!int.TryParse(lines[i], out ID)) ID = 0;
                    if (!int.TryParse(lines[i + 1], out XP)) XP = 0;
                    if (!int.TryParse(lines[i + 2], out Niveau)) Niveau = 0;
                    Atk1 = lines[i + 3];
                    if (!int.TryParse(lines[i + 4], out ForceAtk1)) ForceAtk1 = 0;
                    if (!int.TryParse(lines[i + 5], out UsesLeftAtk1)) UsesLeftAtk1 = 0;
                    Atk2 = i + 6 < lines.Length && !string.IsNullOrWhiteSpace(lines[i + 6]) ? lines[i + 6] : "";
                    if (Atk2 != "" )
                    {
                        if (!int.TryParse(lines[i + 7], out ForceAtk2)) ForceAtk2 = 0;
                        if (!int.TryParse(lines[i + 8], out UsesLeftAtk2)) UsesLeftAtk2 = 0;
                    }
                    else
                    {
                        ForceAtk2 = 0;
                        UsesLeftAtk2 = 0;
                    }

                    // Do something with the retrieved values
                    Console.WriteLine($"Name: {NomPkm}");
                    Console.WriteLine($"Life: {VieMax}");
                    Console.WriteLine($"ID: {ID}");
                    Console.WriteLine($"XP: {XP}");
                    Console.WriteLine($"Level: {Niveau}");
                    Console.WriteLine($"Attaque 1: {Atk1}");
                    Console.WriteLine($"Force attaque 1: {ForceAtk1}");
                    Console.WriteLine($"Nombre d'attaques totales: {UsesLeftAtk1}");

                    // Print AtkName2, AtkStrength2, and AtkRemaining2 only if AtkName2 is not empty or whitespace
                    if (!string.IsNullOrWhiteSpace(Atk2))
                    {
                        Console.WriteLine($"Attaque 2: {Atk2}");
                        Console.WriteLine($"Force attaque 2: {ForceAtk2}");
                        Console.WriteLine($"Nombre d'attaques totales: {UsesLeftAtk2}");
                    }
                    Console.WriteLine("\n");
                    // Break out of the loop since the data has been found
                    break;
                }
                else
                {
                    Console.WriteLine("Not enough lines above or below #1 in the file.");
                    break;
                }
            }
        }
        return null; 
    }

    public static void AddPkmInv()
    {
        string NomPkm = "", Atk1 = "",Atk2 = "";
        int VieMax = 0, ID = 0, XP = 0, Niveau = 0, ForceAtk1 = 0, UsesLeftAtk1 = 0, ForceAtk2 = 0, UsesLeftAtk2 = 0;
        SearchPKM(NomPkm, VieMax, ID, XP, Niveau, Atk1, ForceAtk1, UsesLeftAtk1, Atk2, ForceAtk2, UsesLeftAtk2);

    }
}
