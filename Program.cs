using System;
using System.IO;
using System.Threading.Tasks;

class TxtToStr
{
    static void Main()
    {
        // Chemin du fichier
        string filePath = "C:/Users/rponsignon/source/repos/pokemonconsole/asset/MAP1.txt";

        try
        {
            // Compteur pour le nombre d'occurrences de "@"
            int atSymbolCount = 0;

            // Récup les lignes
            string[] lines = File.ReadAllLines(filePath);


            Console.WriteLine("\n");
            // Parcourir chaque ligne
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                // Parcourir chaque caractère de la ligne
                for (int col = 0; col < line.Length; col++)
                {
                    // Vérifier si le caractère est celui recherché
                    if (line[col] == '@')
                    {
                        // Afficher les coordonnées du caractère trouvé
                        Console.WriteLine($"Coordonnées du joueur:\nX:{col + 1} Y:{row + 1}\n");
                        // Incrémenter le compteur
                        atSymbolCount++;
                    }
                }
            }
            if (atSymbolCount == 0)
            {
                Console.WriteLine("Il n'y a pas de joueur sur la map");
            }
            else if (atSymbolCount == 1)
            {
                // Affiche les lignes dans la console
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }


        }
        catch (IOException e)
        {
            Console.WriteLine("Erreur à l'ouverture du fichier:" + e.Message);
        }
    }
}
