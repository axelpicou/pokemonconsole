using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

class BattleManager
{
    public static void StartBattle()
    {
        //Charge le pokémon du joueur depuis InvJoueurTemp.txt
        Pokemon playerPokemon = LoadPlayerPokemon();

        //Advresaire aléatoire depuis ListePkm.txt
        Pokemon opponentPokemon = SelectRandomOpponent(playerPokemon);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"\nPokémon du joueur:\n{playerPokemon.NomPkm}");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nPokemon Ennemi:\n{opponentPokemon.NomPkm}");
        Console.ResetColor();




        // Boucle Principale
        while (playerPokemon.VieActuelle > 0 && opponentPokemon.VieActuelle > 0)
        {
            // Tour du joueur
            PlayerTurn(playerPokemon, opponentPokemon);

            // Regarde si la vie de l'ennemi est <= 0
            if (opponentPokemon.VieActuelle <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n\n\nBravo!\nVous avez battu {opponentPokemon.NomPkm}!");
                EndBattle(playerPokemon);
                Console.ResetColor();
                break;
            }

            // Tour Adverse
            OpponentTurn(playerPokemon, opponentPokemon);

            // Regarde si la vie du joueur est <= 0
            if (playerPokemon.VieActuelle <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n\n\nOh non! {opponentPokemon.NomPkm} a battu votre {playerPokemon}!");
                Console.ResetColor();
                break;
            }
        }
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\n\nFin du combat.");
        Console.ResetColor();
    }




    private static Pokemon LoadPlayerPokemon()
    {
        // Charge le pokémon du joueur depuis InvJoueurTemp.txt
        string invJoueurTempPath = "../../../asset/InvJoueurTemp.txt";
        string jsonContent = File.ReadAllText(invJoueurTempPath);
        Pokemon playerPokemon = JsonSerializer.Deserialize<Pokemon>(jsonContent);

        // Adjust opponent Pokémon stats based on level
        AdjustPokemonStatsByLevel(playerPokemon);

        return playerPokemon;
    }

    private static Pokemon SelectRandomOpponent(Pokemon playerPokemon)
    {
        // Advresaire aléatoire depuis ListePkm.txt
        string listePkmPath = "../../../asset/ListePkm.txt";
        string[] lines = File.ReadAllLines(listePkmPath);

        // ID random entre 0 et 5
        Random random = new Random();
        int randomId = random.Next(0, 6); // 6 Exclusif > entre 0 =< x < 6

        // Trouve l'index de l'id cherchée
        int indexOfPokemon = Array.IndexOf(lines, "#" + randomId.ToString());

        
        while (indexOfPokemon == -1)
        {
            randomId = random.Next(0, 6);
            indexOfPokemon = Array.IndexOf(lines, "#" + randomId.ToString());
        }

        try
        {
            // Calculer le niveau de l'opposant en utilisant la nouvelle méthode
            int opponentLevel = CalculateOpponentLevel(playerPokemon.Niveau);

            // Création d'une instance
            Pokemon opponentPokemon = new Pokemon
            {
                NomPkm = lines.ElementAtOrDefault(indexOfPokemon - 2),
                VieMax = int.Parse(lines.ElementAtOrDefault(indexOfPokemon - 1) ?? "0"),
                VieActuelle = int.Parse(lines.ElementAtOrDefault(indexOfPokemon - 1) ?? "0"),
                ID = lines.ElementAtOrDefault(indexOfPokemon),
                XP = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 1) ?? "0"),
                Niveau = opponentLevel,
                Atk1 = lines.ElementAtOrDefault(indexOfPokemon + 3),
                ForceAtk1 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 4) ?? "0"),
                UsesLeftAtk1 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 5) ?? "0"),
                Atk2 = lines.ElementAtOrDefault(indexOfPokemon + 6),
                ForceAtk2 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 7) ?? "0"),
                UsesLeftAtk2 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 8) ?? "0"),
            };
            // Adjust opponent Pokémon stats based on level
            AdjustPokemonStatsByLevel(opponentPokemon);
            return opponentPokemon;
        }
        catch (FormatException e)
        {
            Console.WriteLine($"FormatException in SelectRandomOpponent: {e.Message}");
            Console.WriteLine($"Problematic line: {lines.ElementAtOrDefault(indexOfPokemon)}");
            throw;
        }
    }

    private static void PlayerTurn(Pokemon playerPokemon, Pokemon opponentPokemon)
    {
        // Logique de l'attaque pour le joueur
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\n\n\n\nTour du joueur:\n");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Votre Pokémon:");
        DisplayPokemonInfo(playerPokemon);
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n\nPokémon Adverse:");
        DisplayPokemonInfo(opponentPokemon);
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n\nChoisissez une attaque: \n\n1. {playerPokemon.Atk1}, Puissance: {playerPokemon.ForceAtk1}\nUtilisations restantes: {playerPokemon.UsesLeftAtk1}\n\n2. {playerPokemon.Atk2}, Puissance: {playerPokemon.ForceAtk2}\nUtilisations restantes: {playerPokemon.UsesLeftAtk2}\n");
        int choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
                PerformAttack(playerPokemon, opponentPokemon, playerPokemon.Atk1, playerPokemon.ForceAtk1);
                Console.ResetColor();
                Console.Clear();
                break;
            case 2:
                PerformAttack(playerPokemon, opponentPokemon, playerPokemon.Atk2, playerPokemon.ForceAtk2);
                Console.ResetColor();
                Console.Clear();
                break;
            default:
                Console.WriteLine("\n\n\nChoix invalide! Le Pokémon n'as rien fait");
                Console.ResetColor();
                Console.Clear();
                break;
        }
    }

    private static void OpponentTurn(Pokemon playerPokemon, Pokemon opponentPokemon)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        // Logique de l'attaque pour l'ennemi 
        Console.WriteLine("\n\nTour Ennemi:");

        // Attaque Random pour l'ennemi
        Random random = new Random();
        int attackChoice = random.Next(1, 3); 

        if (attackChoice == 1)
        {
            PerformAttack(opponentPokemon, playerPokemon, opponentPokemon.Atk1, opponentPokemon.ForceAtk1);
        }
        else if (attackChoice == 2)
        {
            PerformAttack(opponentPokemon, playerPokemon, opponentPokemon.Atk2, opponentPokemon.ForceAtk2);
        }
        
        Console.ResetColor();
    }

    private static void PerformAttack(Pokemon attacker, Pokemon target, string attackName, int attackStrength)
    {
        // Logique de l'attaque
        Console.WriteLine($"\n\n{attacker.NomPkm} a utilisé {attackName}!");

        if (attackName == attacker.Atk1 && attacker.UsesLeftAtk1 > 0)
    {
        // Perform the attack
        target.VieActuelle -= attackStrength;
        Console.WriteLine($"{target.NomPkm} a pris {attackStrength} dégàts!");
        attacker.UsesLeftAtk1--;

        Console.WriteLine($"Utilisations restante de {attackName}: {attacker.UsesLeftAtk1}");
    }
    else if (attackName == attacker.Atk2 && attacker.UsesLeftAtk2 > 0)
    {
        // Perform the attack
        target.VieActuelle -= attackStrength;
        Console.WriteLine($"{target.NomPkm} takes {attackStrength} damage!");
        attacker.UsesLeftAtk2--;

        Console.WriteLine($"Utilisations restante de {attackName}: {attacker.UsesLeftAtk2}");
    }
    else
    {
        Console.WriteLine($"L'attaque {attackName} de {attacker.NomPkm} n'est plus utilisable!");
    }
        Console.WriteLine($"\n{target.NomPkm} a pris {attackStrength} dégàts.\nPV restants: {target.VieActuelle}");
    }

    private static int CalculateDamage(Pokemon attacker, int attackStrength)
    {
        // Calcul de DPS selon les niveaux, puissance et autre
        int damage = attackStrength * attacker.Niveau;

        return damage;
    }

    private static void DisplayPokemonInfo(Pokemon pokemon)
    {
        // Info des Pokémons
        Console.WriteLine($"{pokemon.NomPkm}, Niveau {pokemon.Niveau}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nPV: {pokemon.VieActuelle}/{pokemon.VieMax}\n");
        Console.ResetColor();
    }

    private static void EndBattle(Pokemon playerPokemon)
    {
        // ... (other logic)

        // Award a random amount of XP after the battle
        Random random = new Random();
        int xpEarned = random.Next(50, 100); // Adjust the range based on your game's balancing
        // Add the earned XP to the player's Pokémon
        playerPokemon.XP += xpEarned;

        Console.WriteLine($"\n\nVotre {playerPokemon.NomPkm} a gagné {xpEarned} d'XP!\n{playerPokemon.XP}/100\n\n");


        // Reset current health to max health
        playerPokemon.VieActuelle = playerPokemon.VieMax;

        // Reset the number of uses for both abilities to their defaults
        ResetAbilitiesUses(playerPokemon);

        // Check if the player's Pokémon leveled up
        InventoryManager.LevelUp(playerPokemon);

        // Save the updated level and XP to InvJoueurTemp.txt
        InventoryManager.SaveLevelAndXP(playerPokemon);
    }

    private static void ResetAbilitiesUses(Pokemon pokemon)
    {
        // Retrieve default values from ListePkm.txt based on the Pokémon's ID
        string listePkmPath = "../../../asset/ListePkm.txt";
        string[] lines = File.ReadAllLines(listePkmPath);

        // Find the index of the selected Pokemon ID
        int indexOfPokemon = Array.IndexOf(lines, pokemon.ID);

        if (indexOfPokemon != -1)
        {
            // Reset the number of uses for both abilities to their defaults
            pokemon.UsesLeftAtk1 = int.TryParse(lines[indexOfPokemon + 5], out int usesAtk1) ? usesAtk1 : 0;
            pokemon.UsesLeftAtk2 = int.TryParse(lines[indexOfPokemon + 8], out int usesAtk2) ? usesAtk2 : 0;
        }
        else
        {
            Console.WriteLine($"Error: Pokémon with ID {pokemon.ID} not found in ListePkm.txt");
        }
    }

    private static void AdjustPokemonStatsByLevel(Pokemon pokemon)
    {
        Random random = new Random();
        // Adjust Pokémon stats based on their level
        // You can define your own formula for adjusting the stats here
        pokemon.VieMax += 10 * pokemon.Niveau; // Adjust max health
        pokemon.VieActuelle = pokemon.VieMax;   // Reset current health to max health
        pokemon.ForceAtk1 += 5 * pokemon.Niveau; // Adjust attack strength for Atk1
        pokemon.ForceAtk2 += 3 * pokemon.Niveau; // Adjust attack strength for Atk2

    }
    private static int CalculateOpponentLevel(int playerPokemonLevel)
{
    // Calculate opponent level based on player's level with a slight random offset
    Random random = new Random();
    int levelOffset = random.Next(-2, 3); // Adjust the offset range based on your game's balancing
    int calculatedLevel = playerPokemonLevel + levelOffset;

    // Ensure the opponent's level is within a specific range and enforce a minimum level of 1
    return Math.Max(1, Math.Min(calculatedLevel, playerPokemonLevel + 2));
}

}