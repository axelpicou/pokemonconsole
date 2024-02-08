using System;
using System.IO;
using System.Linq;
using System.Text.Json;

class BattleManager
{
    public static void StartBattle()
    {
        // Load player's Pokemon from InvJoueurTemp.txt
        Pokemon playerPokemon = LoadPlayerPokemon();

        // Select a random Pokemon for the opponent from ListePkm.txt
        Pokemon opponentPokemon = SelectRandomOpponent();

        Console.WriteLine($"Player's Pokemon: {playerPokemon.NomPkm}");
        Console.WriteLine($"Opponent's Pokemon: {opponentPokemon.NomPkm}");

        // Main battle loop
        while (playerPokemon.VieActuelle > 0 && opponentPokemon.VieActuelle > 0)
        {
            // Player's turn
            PlayerTurn(playerPokemon, opponentPokemon);

            // Check if opponent is defeated
            if (opponentPokemon.VieActuelle <= 0)
            {
                Console.WriteLine($"Congratulations! You defeated {opponentPokemon.NomPkm}!");
                break;
            }

            // Opponent's turn
            OpponentTurn(playerPokemon, opponentPokemon);

            // Check if player is defeated
            if (playerPokemon.VieActuelle <= 0)
            {
                Console.WriteLine($"Oh no! {opponentPokemon.NomPkm} defeated your Pokemon!");
                break;
            }
        }

        Console.WriteLine("Battle ended.");
    }

    private static Pokemon LoadPlayerPokemon()
    {
        // Load player's Pokemon from InvJoueurTemp.txt
        // You may want to add error handling here
        string invJoueurTempPath = "../../../asset/InvJoueurTemp.txt";
        string jsonContent = File.ReadAllText(invJoueurTempPath);
        Pokemon playerPokemon = JsonSerializer.Deserialize<Pokemon>(jsonContent);

        return playerPokemon;
    }

    private static Pokemon SelectRandomOpponent()
    {
        // Select a random Pokemon for the opponent from ListePkm.txt
        // You may want to add error handling here
        string listePkmPath = "../../../asset/ListePkm.txt";
        string[] lines = File.ReadAllLines(listePkmPath);

        // Choose a random ID between 0 and 5
        Random random = new Random();
        int randomId = random.Next(0, 6); // 6 is exclusive, so it will be between 0 and 5

        // Find the index of the selected Pokemon ID
        int indexOfPokemon = Array.IndexOf(lines, "#" + randomId.ToString());

        // If the index is not found, try again (you may want to add additional handling)
        while (indexOfPokemon == -1)
        {
            randomId = random.Next(0, 6);
            indexOfPokemon = Array.IndexOf(lines, "#" + randomId.ToString());
        }

        try
        {
            // Create a Pokemon object from the selected line
            Pokemon opponentPokemon = new Pokemon
            {
                NomPkm = lines.ElementAtOrDefault(indexOfPokemon - 2),
                VieMax = int.Parse(lines.ElementAtOrDefault(indexOfPokemon - 1) ?? "0"),
                VieActuelle = int.Parse(lines.ElementAtOrDefault(indexOfPokemon - 1) ?? "0"),
                ID = lines.ElementAtOrDefault(indexOfPokemon),
                XP = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 1) ?? "0"),
                Niveau = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 2) ?? "0"),
                Atk1 = lines.ElementAtOrDefault(indexOfPokemon + 3),
                ForceAtk1 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 4) ?? "0"),
                UsesLeftAtk1 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 5) ?? "0"),
                Atk2 = lines.ElementAtOrDefault(indexOfPokemon + 6),
                ForceAtk2 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 7) ?? "0"),
                UsesLeftAtk2 = int.Parse(lines.ElementAtOrDefault(indexOfPokemon + 8) ?? "0"),
            };

            return opponentPokemon;
        }
        catch (FormatException e)
        {
            Console.WriteLine($"FormatException in SelectRandomOpponent: {e.Message}");
            Console.WriteLine($"Problematic line: {lines.ElementAtOrDefault(indexOfPokemon)}");
            throw; // Rethrow the exception after logging
        }
    }

    private static void PlayerTurn(Pokemon playerPokemon, Pokemon opponentPokemon)
    {
        // Implement player's turn logic here
        // For example, let the player choose an attack
        Console.WriteLine("Player's Turn:");
        DisplayPokemonInfo(playerPokemon);
        DisplayPokemonInfo(opponentPokemon);

        Console.WriteLine("Choose an attack: 1. Attack 1, 2. Attack 2");
        int choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
                PerformAttack(playerPokemon, opponentPokemon, playerPokemon.Atk1, playerPokemon.ForceAtk1);
                break;
            case 2:
                PerformAttack(playerPokemon, opponentPokemon, playerPokemon.Atk2, playerPokemon.ForceAtk2);
                break;
            default:
                Console.WriteLine("Invalid choice. Your Pokemon hesitated!");
                break;
        }
    }

    private static void OpponentTurn(Pokemon playerPokemon, Pokemon opponentPokemon)
    {
        // Implement opponent's turn logic here
        Console.WriteLine("Opponent's Turn:");

        // Randomly select an attack for the opponent
        Random random = new Random();
        int attackChoice = random.Next(1, 3); // Assuming the opponent has two attacks

        if (attackChoice == 1)
        {
            PerformAttack(opponentPokemon, playerPokemon, opponentPokemon.Atk1, opponentPokemon.ForceAtk1);
        }
        else if (attackChoice == 2)
        {
            PerformAttack(opponentPokemon, playerPokemon, opponentPokemon.Atk2, opponentPokemon.ForceAtk2);
        }
        // Add more conditions for additional attacks if necessary
    }

    private static void PerformAttack(Pokemon attacker, Pokemon target, string attackName, int attackStrength)
    {
        // Implement attack logic here
        Console.WriteLine($"{attacker.NomPkm} used {attackName}!");

        // Calculate damage and reduce target's health
        int damage = CalculateDamage(attacker, attackStrength);
        target.VieActuelle -= damage;

        Console.WriteLine($"{target.NomPkm} took {damage} damage. {target.NomPkm}'s remaining health: {target.VieActuelle}");
    }

    private static int CalculateDamage(Pokemon attacker, int attackStrength)
    {
        // Implement damage calculation logic based on attacker's level, attack strength, etc.
        // For simplicity, a basic formula is used here
        int damage = attackStrength * attacker.Niveau;

        return damage;
    }

    private static void DisplayPokemonInfo(Pokemon pokemon)
    {
        // Display Pokemon information
        Console.WriteLine($"{pokemon.NomPkm} - Level: {pokemon.Niveau}, Health: {pokemon.VieActuelle}/{pokemon.VieMax}\n");
    }
}