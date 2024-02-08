// Map.cs
using System;
using System.IO;

class Map
{
    public static void MovePlayer(string[] lines, ref Position position, int offsetX, int offsetY, string[] initialMap)
    {
        // Calculer la nouvelle position
        int newX = position.X + offsetX;
        int newY = position.Y + offsetY;

        // Vérifier les limites
        if (newY >= 0 && newY < lines.Length && newX >= 0 && newX < lines[newY].Length && (lines[newY][newX] == '.' || lines[newY][newX] == '/'))
        {
            // Réinitialiser la cellule à l'ancienne position
            ResetCell(lines, position, initialMap);

            // Déplacer le joueur
            position.X = newX;
            position.Y = newY;

            if (lines[newY][newX] == '/')
            {
                // Si la case contient '/'
                Console.WriteLine("Tu touche de l'Herbe omg");
                // Introduce a random chance of triggering a fight

                if (RandomChance(0.15)) // Adjust the probability as needed (e.g., 0.3 for a 30% chance)
                {
                    Console.WriteLine("A wild Pokémon appeared!");
                    // Start a battle using the BattleManager
                    BattleManager.StartBattle();
                }
            }

            // Mettre à jour la carte à la nouvelle position
            char[] lineChars = lines[newY].ToCharArray();
            lineChars[newX] = '@';
            lines[newY] = new string(lineChars);
        }
    }
    private static bool RandomChance(double probability)
    {
        // Generate a random number between 0 and 1
        double randomValue = new Random().NextDouble();

        // Check if the random value is less than the specified probability
        return randomValue < probability;
    }

    public static void ResetCell(string[] lines, Position position, string[] initialMap)
    {
        char initialCell = initialMap[position.Y][position.X];
        lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, initialCell.ToString());
    }

    // Fonction pour effacer l'affichage du joueur à l'ancienne position
    public static void ClearPlayer(string[] lines, Position position)
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