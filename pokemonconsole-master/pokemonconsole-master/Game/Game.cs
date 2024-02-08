// Game.cs
using System;
using System.IO;
using pokemonconsole.Game;

class Game
{
    static void Main()
    {
        // Chemin du fichier
        string filePath = "../../../asset/MAP1.txt";
        string playersavefile = "../../../save/playerPOS.txt";
        string[] InvActuel = File.ReadAllLines("../../../asset/InvModif.txt");
        string[] InvInitial = File.ReadAllLines("../../../asset/InvInitial.txt");
        InvActuel = InvInitial;

        BattleManager battleManager = new BattleManager();
        Map.InitializeBattleManager(battleManager);

        // Coordonnées initiales du joueur
        Position playerPosition;

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
                        Map.MovePlayer(initialLines, ref playerPosition, 0, -1, initialMap, initialLines);
                        break;
                    case ConsoleKey.Q:
                        Map.MovePlayer(initialLines, ref playerPosition, -1, 0, initialMap, initialLines);
                        break;
                    case ConsoleKey.S:
                        Map.MovePlayer(initialLines, ref playerPosition, 0, 1, initialMap, initialLines);
                        break;
                    case ConsoleKey.D:
                        Map.MovePlayer(initialLines, ref playerPosition, 1, 0, initialMap, initialLines);
                        break;
                    case ConsoleKey.I:
                        InventoryManager.DisplayInventory(InvActuel);
                        InvOpen = 1;
                        break;
                }

                Player.SavePosition(playersavefile, playerPosition);

                if (InvOpen == 0)
                {
                    // Afficher la carte mise à jour
                    Console.WriteLine($"Le joueur est en X:{playerPosition.X} Y:{playerPosition.Y}");
                    Console.WriteLine("Carte mise à jour :");
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
