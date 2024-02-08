// Map.cs
using System;

namespace pokemonconsole.Game
{
    class Map
    {
        private static Random random = new Random();
        private static BattleManager battleManager = new BattleManager();

        public static void InitializeBattleManager(BattleManager manager)
        {
            battleManager = manager;
        }

        public static void MovePlayer(string[] lines, ref Position position, int offsetX, int offsetY, string[] initialMap, string[] initialLines)
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

                // Générer un nombre aléatoire entre 1 et 100
                int randomNumber = random.Next(1, 101);

                // Vérifier si le nombre aléatoire est inférieur ou égal à 20 (20 % de chance)
                if (randomNumber <= 20 && lines[newY][newX] == '/')
                {
                    // Commencer une bataille
                    battleManager.StartBattle(initialLines, position);
                }

                // Mettre à jour la carte à la nouvelle position
                char[] lineChars = lines[newY].ToCharArray();
                lineChars[newX] = '@';
                lines[newY] = new string(lineChars);
            }
        }

        private static void ResetCell(string[] lines, Position position, string[] initialMap)
        {
            char initialCell = initialMap[position.Y][position.X];
            lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, initialCell.ToString());
        }

        public static void ClearPlayer(string[] lines, Position position)
        {
            // Réinitialiser la cellule à l'emplacement du joueur
            char currentCell = lines[position.Y][position.X];
            lines[position.Y] = lines[position.Y].Remove(position.X, 1).Insert(position.X, currentCell.ToString());
        }
    }
}
