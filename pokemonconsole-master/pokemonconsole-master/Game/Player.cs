// Player.cs
using System;
using System.IO;

struct Position
{
    public int X;
    public int Y;
}

class Player
{
    public static void SavePosition(string filePath, Position position)
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

    public static Position LoadPosition(string filePath)
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
}