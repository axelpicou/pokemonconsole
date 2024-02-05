using System;
using System.IO;

struct Position
{
    public int X;
    public int Y;
}

class Map
{
    static void Main()
    {
        // Chemin du fichier
        string filePath = "../../../asset/MAP1.txt";
        string playersavefile = "../../../save/playerPOS.txt";

        // Coordonnées initiales du joueur
        Position playerPosition;

        try
        {         
            
            // Lire les lignes du fichier pour obtenir la carte initiale
            string[] initialLines = File.ReadAllLines(filePath);

            // Afficher la carte initiale
            Console.WriteLine("Carte initiale :");
            foreach (string line in initialLines)
            {
                Console.WriteLine(line);
            }

            // Charger les coordonnées du joueur à partir du fichier
            playerPosition = LoadPlayerPosition(playersavefile);

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true); // Attendre l'entrée de l'utilisateur

                // Effacer la console à chaque déplacement
                Console.Clear();

                // Effacer l'affichage précédent du joueur
                ClearPlayer(initialLines, playerPosition);

                // Mettre à jour les coordonnées du joueur en fonction de la touche appuyée
                switch (key.Key)
                {
                    case ConsoleKey.Z:
                        MovePlayer(initialLines, ref playerPosition, 0, -1);
                        break;
                    case ConsoleKey.Q:
                        MovePlayer(initialLines, ref playerPosition, -1, 0);
                        break;
                    case ConsoleKey.S:
                        MovePlayer(initialLines, ref playerPosition, 0, 1);
                        break;
                    case ConsoleKey.D:
                        MovePlayer(initialLines, ref playerPosition, 1, 0);
                        break;
                }

                SavePlayerPosition(playersavefile, playerPosition);


                // Afficher la carte mise à jour
                Console.WriteLine($"Le joueur est en X:{playerPosition.X} Y:{playerPosition.Y}");
                Console.WriteLine("Carte mise à jour :");
                foreach (string line in initialLines)
                {
                    Console.WriteLine(line);
                }
            } while (key.Key != ConsoleKey.Escape); // Sortir de la boucle si la touche Escape est appuyée
        }
        catch (IOException e)
        {
            Console.WriteLine("Erreur à l'ouverture ou l'écriture du fichier : " + e.Message);
        }
    }

    // Fonction pour déplacer le joueur et vérifier les limites
    static void MovePlayer(string[] lines, ref Position position, int offsetX, int offsetY)
    {
        // Calculer la nouvelle position
        int newX = position.X + offsetX;
        int newY = position.Y + offsetY;

        // Vérifier les limites
        if (newY >= 0 && newY < lines.Length && newX >= 0 && newX < lines[newY].Length && (lines[newY][newX] == '.' || lines[newY][newX] == '/'))
        {
            // Mettre à jour la carte à l'ancienne position
            lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, ".");

            // Déplacer le joueur
            position.X = newX;
            position.Y = newY;

            if (lines[newY][newX] == '/')
            {
                // Si la case contient '/'
                Console.WriteLine("Caca");
            }

            // Mettre à jour la carte à la nouvelle position
            char[] lineChars = lines[newY].ToCharArray();
            lineChars[newX] = '@';
            lines[newY] = new string(lineChars);
        }
    }
    static void SavePlayerPosition(string filePath, Position position)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"{position.X},{position.Y}");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Erreur à l'écriture du fichier de sauvegarde : " + e.Message);
        }
    }
    // Fonction pour charger la position du joueur depuis un fichier
    static Position LoadPlayerPosition(string filePath)
    {
        Position position = new Position();

        try
        {
            if (File.Exists(filePath))
            {
                string[] coordinates = File.ReadAllText(filePath).Split(',');
                if (coordinates.Length == 2 && int.TryParse(coordinates[0], out position.X) && int.TryParse(coordinates[1], out position.Y))
                {
                    Console.WriteLine("Position du joueur chargée depuis le fichier.");
                }
                else
                {
                    Console.WriteLine("Le fichier de position du joueur est mal formaté. Utilisation des coordonnées par défaut.");
                }
            }
            else
            {
                Console.WriteLine("Le fichier de position du joueur n'existe pas. Utilisation des coordonnées par défaut.");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Erreur lors de la lecture du fichier de position du joueur : " + e.Message);
        }

        return position;
    }

    // Fonction pour effacer l'affichage du joueur à l'ancienne position
    static void ClearPlayer(string[] lines, Position position)
    {
        char currentCell = lines[position.Y][position.X];

        // Si la cellule d'origine était '/', la rétablir en tant que '/'
        if (currentCell == '@' && lines[position.Y][position.X] == '.')
        {
            lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, "/");
        }
        else
        {
            // Sinon, rétablir la cellule d'origine (qui était probablement '.')
            lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, currentCell.ToString());
        }
    }
}
