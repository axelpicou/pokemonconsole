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
        string[] InvActuel = File.ReadAllLines("../../../asset/InvModif.txt");
        string[] InvInitial = File.ReadAllLines("../../../asset/InvInitial.txt");
        InvActuel = InvInitial;

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

            int InvOpen = 0;
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
                    case ConsoleKey.I:
                        Inv(InvActuel);
                        InvOpen = 1;
                        break;
                }

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

    static void Inv(string[] InvJoueur)
    {
        ConsoleKeyInfo key;
        int pointerPos = 1;

        do
        {
            Console.Clear();
            // Display the inventory with the cursor highlighting the selected slot
            for (int i = 0; i < InvJoueur.Length; i++)
            {
                if (i == pointerPos)
                {

                    // Change color for the four lines below the selected slot
                    for (int j = 0; j <= 3 && i + j < InvJoueur.Length; j++)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(InvJoueur[i + j]);
                        Console.ResetColor();
                    }

                    // Skip the next four lines
                    i += 3;
                }
                else
                {
                    Console.WriteLine(InvJoueur[i]);
                }
            }

            key = Console.ReadKey(true); // Wait for user input

            // Update the cursor position based on user input
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    pointerPos = Math.Max(1, pointerPos - 4); // Move the cursor up
                    break;
                case ConsoleKey.DownArrow:
                    pointerPos = Math.Min(InvJoueur.Length - 5, pointerPos + 4); // Move the cursor down
                    break;



            }
        } while (key.Key != ConsoleKey.I); // Sortir de la boucle si la touche Escape est appuyée
        Console.Clear() ;   
    }
}
