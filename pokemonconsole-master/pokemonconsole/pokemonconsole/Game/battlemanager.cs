namespace pokemonconsole.Game
{
    internal class BattleManager
    {
        public void StartBattle(string[] initialLines, Position playerPosition)
        {
            // Effacez l'écran et affichez le mot "Combat"
            Console.Clear();
            Console.WriteLine("Combat");

            // Ajoutez ici la logique supplémentaire de la bataille
            string[] options = { "Attaquer", "Utiliser un objet", "Fuir", "Changer de Pokémon" };
            Console.WriteLine("Vous êtes attaqué par un Pokémon sauvage !");
            Console.WriteLine("Choisissez une action :");

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            int choice = -1;
            while (choice < 1 || choice > options.Length)
            {
                Console.Write("Votre choix : ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                    continue;
                }
            }

            // Réagir en fonction du choix du joueur
            switch (choice)
            {
                case 1:
                    // Attaquer
                    // À compléter avec la logique de l'attaque
                    break;
                case 2:
                    // Utiliser un objet
                    // À compléter avec la logique de l'utilisation d'objet
                    break;
                case 3:
                    // Fuir
                    // À compléter avec la logique de la fuite
                    break;
                case 4:
                    // Changer de Pokémon
                    // À compléter avec la logique du changement de Pokémon
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }

            // Réinitialiser la position du joueur après la bataille
            Map.ClearPlayer(initialLines, playerPosition);
        }
    }
}
