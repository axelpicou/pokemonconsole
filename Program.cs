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
        string filePath = "C:/Users/apicou/source/repos/pokemonconsole/asset/MAP1.txt";

        // Coordonnées initiales du joueur
        Position playerPosition = new Position { X = 15, Y = 12 };

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
        if (newY >= 0 && newY < lines.Length && newX >= 0 && newX < lines[newY].Length && lines[newY][newX] == '.')
        {
            // Mettre à jour la carte à l'ancienne position
            lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, ".");

            // Déplacer le joueur
            position.X = newX;
            position.Y = newY;

            // Mettre à jour la carte à la nouvelle position
            char[] lineChars = lines[newY].ToCharArray();
            lineChars[newX] = '@';
            lines[newY] = new string(lineChars);
        }
    }

    // Fonction pour effacer l'affichage du joueur à l'ancienne position
    static void ClearPlayer(string[] lines, Position position)
    {
        lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, ".");
    }
}
