// Program.cs
using System;
using System.IO;


class Game
{
    static void Main()
    {
        // Chemin du fichier
        File.Delete("../../../asset/InvJoueurTemp.txt");
        string filePath = "../../../asset/MAP1.txt";
        string playersavefile = "../../../save/playerPOS.txt";
        string[] InvActuel = File.ReadAllLines("../../../asset/InvModif.txt");
        string[] InvInitial = File.ReadAllLines("../../../asset/InvInitial.txt");
        InvActuel = InvInitial;

        // Coordonnées initiales du joueur
        Position playerPosition;
        Pokemon foundPokemon = InventoryManager.SearchPKM();
        if (foundPokemon != null)
        {
            // Use the found Pokémon's information as needed
            Console.WriteLine($"\nPokémon Choisi: {foundPokemon.NomPkm}");
            Console.WriteLine($"VieMax: {foundPokemon.VieMax}HP");
            Console.WriteLine($"Première attaque: {foundPokemon.Atk1}");
            Console.WriteLine($"Puissance: {foundPokemon.ForceAtk1}");
            Console.WriteLine($"Coups totaux: {foundPokemon.UsesLeftAtk1}");
            if (foundPokemon.Atk2 != "")
            {
                Console.WriteLine($"Deuxième attaque: {foundPokemon.Atk2}");
                Console.WriteLine($"Puissance: {foundPokemon.ForceAtk2}");
                Console.WriteLine($"Coups totaux: {foundPokemon.UsesLeftAtk2}");
            }
            Console.WriteLine("\n");
            // ... (other properties)
        }
        else
        {
            Console.WriteLine("Pokémon not found.\n");
        }
        // Lire les lignes du fichier pour obtenir la carte initiale
        string[] initialLines = File.ReadAllLines(filePath);

        // Sauvegarder la carte initiale pour pouvoir réinitialiser les cellules
        string[] initialMap = new string[initialLines.Length];
        Array.Copy(initialLines, initialMap, initialLines.Length);

        try
        {
            // Charger les coordonnées du joueur à partir du fichier
            playerPosition = Player.LoadPosition(playersavefile);

            // Afficher la carte initiale
            Console.WriteLine("Carte initiale :");
            foreach (string line in initialLines)
            {
                Console.WriteLine(line);
            }

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true); // Attendre l'entrée de l'utilisateur
                int InvOpen = 0;

                // Effacer la console à chaque déplacement
                Console.Clear();

                // Effacer l'affichage précédent du joueur
                Map.ClearPlayer(initialLines, playerPosition);

                // Mettre à jour les coordonnées du joueur en fonction de la touche appuyée
                switch (key.Key)
                {
                    case ConsoleKey.Z:
                        Map.MovePlayer(initialLines, ref playerPosition, 0, -1, initialMap);
                        break;
                    case ConsoleKey.Q:
                        Map.MovePlayer(initialLines, ref playerPosition, -1, 0, initialMap);
                        break;
                    case ConsoleKey.S:
                        Map.MovePlayer(initialLines, ref playerPosition, 0, 1, initialMap);
                        break;
                    case ConsoleKey.D:
                        Map.MovePlayer(initialLines, ref playerPosition, 1, 0, initialMap);
                        break;
                    case ConsoleKey.P:
                        // Use File.Delete to delete the file
                        File.Delete("../../../asset/Save.txt");
                        
                        break;
                    /*case ConsoleKey.I:
                        InventoryManager.DisplayInventory(InvActuel);
                        InvOpen = 1;
                        break;*/
                    case ConsoleKey.L:
                        InventoryManager.SaveToTxt(foundPokemon);
                            break;
                }

                Player.SavePosition(playersavefile, playerPosition);

                if (InvOpen == 0)
                {
                    // Afficher la carte mise à jour
                    /*Console.WriteLine($"\nLe joueur est en X:{playerPosition.X} Y:{playerPosition.Y}");
                    Console.WriteLine("Carte mise à jour :");*/
                    foreach (string line in initialLines)
                    {
                        Console.WriteLine(line);
                    }
                }
            } while (key.Key != ConsoleKey.Escape); // Sortir de la boucle si la touche Escape est appuyée
        }
        catch (IOException e)
        {
            Console.WriteLine("Erreur à l'ouverture ou l'écriture du fichier : " + e.Message);
        }
    }
}