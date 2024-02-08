using System;
using System.Collections.Generic;

namespace pokemonconsole.Game
{
    internal class BattleManager
    {
        private class Random;
         static private Random random = new Random();

        // Ajout de la classe Character pour représenter les personnages
        public class Character
        {
            public string Name { get; set; }
            public CharacterType Type { get; set; }
            public int Level { get; set; }
            public int HP { get; set; }
            public int MP { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Speed { get; set; }

            public void ShowStats()
            {
                Console.WriteLine($"{Name} (Level {Level}) - HP: {HP}/{MaxHP}, MP: {MP}/{MaxMP}, Attack: {Attack}, Defense: {Defense}, Speed: {Speed}");
            }

            public int MaxHP { get; set; }
            public int MaxMP { get; set; }

            public List<string> Attacks { get; set; }
            public List<string> Magic { get; set; }

            public Character(string name, CharacterType type)
            {
                Name = name;
                Type = type;
                Level = 1;
                HP = MaxHP = 100;
                MP = MaxMP = 50;
                Attack = 20;
                Defense = 10;
                Speed = 15;
                Attacks = new List<string> { "Slash", "Bash" };
                Magic = new List<string> { "Fireball", "Heal" };
            }

            public void LevelUp()
            {
                Level++;
                MaxHP += 20;
                MaxMP += 10;
                Attack += 5;
                Defense += 3;
                Speed += 2;
                Console.WriteLine($"{Name} leveled up to Level {Level}!");
            }
        }

        // Ajout de la classe Battle pour gérer les combats
        public class Battle
        {
            private Character player;
            private Character enemy;

            public Battle(Character player, Character enemy)
            {
                this.player = player;
                this.enemy = enemy;
            }

            public void StartBattle()
            {
                Console.WriteLine("Battle Start!");
                Console.WriteLine($"{player.Name} vs {enemy.Name}\n");

                while (player.HP > 0 && enemy.HP > 0)
                {
                    playerTurn();
                    if (enemy.HP <= 0) break;

                    enemyTurn();
                    if (player.HP <= 0) break;
                }

                if (player.HP <= 0)
                {
                    Console.WriteLine("Game Over! All characters are KO.");
                }
                else
                {
                    Console.WriteLine($"Victory! {player.Name} defeated {enemy.Name}!");
                    player.LevelUp();
                }
            }

            private void playerTurn()
            {
                Console.WriteLine("Player's Turn:");
                player.ShowStats();
                ShowActions();

                int choice = GetUserChoice();

                switch (choice)
                {
                    case (int)ActionType.Attack:
                        PerformAttack(player, enemy);
                        break;
                    case (int)ActionType.Magic:
                        PerformMagic(player, enemy);
                        break;
                    case (int)ActionType.Item:
                        // ajouter item logic ici
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        playerTurn();
                        break;
                }
            }

            private void enemyTurn()
            {
                Console.WriteLine("\nEnemy's Turn:");
                PerformAIAction(enemy, player);
            }

            private void ShowActions()
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine($"1. Attack");
                Console.WriteLine($"2. Magic");
                Console.WriteLine($"3. Item");
            }

            private int GetUserChoice()
            {
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
                }
                return choice;
            }

            private void PerformAttack(Character attacker, Character target)
            {
                Console.WriteLine($"Choose an attack:");
                for (int i = 0; i < attacker.Attacks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {attacker.Attacks[i]}");
                }

                int choice = GetUserChoice() - 1;
                int damage = CalculateDamage(attacker.Attack, target.Defense);

                Console.WriteLine($"{attacker.Name} used {attacker.Attacks[choice]}!");
                Console.WriteLine($"Dealt {damage} damage to {target.Name}.");

                target.HP -= damage;
                if (target.HP < 0) target.HP = 0;

                Console.WriteLine($"{target.Name}'s HP: {target.HP}/{target.MaxHP}\n");
            }

            private void PerformMagic(Character caster, Character target)
            {
                Console.WriteLine($"Choose a spell:");
                for (int i = 0; i < caster.Magic.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {caster.Magic[i]}");
                }

                int choice = GetUserChoice() - 1;
                int manaCost = 10; // mettre un coste de mana fix
                if (caster.MP < manaCost)
                {
                    Console.WriteLine("Not enough MP to cast the spell. Choose another action.");
                    playerTurn();
                    return;
                }

                Console.WriteLine($"{caster.Name} cast {caster.Magic[choice]}!");
                caster.MP -= manaCost;

                if (caster.Magic[choice] == "Heal")
                {
                    int healingAmount = CalculateHealing(caster.Attack);
                    caster.HP += healingAmount;
                    Console.WriteLine($"{caster.Name} healed for {healingAmount} HP.");
                }
                else
                {
                    int damage = CalculateDamage(caster.Attack, target.Defense);
                    Console.WriteLine($"Dealt {damage} damage to {target.Name}.");
                    target.HP -= damage;
                    if (target.HP < 0) target.HP = 0;
                }

                Console.WriteLine($"{caster.Name}'s HP: {caster.HP}/{caster.MaxHP}, MP: {caster.MP}/{caster.MaxMP}\n");
            }

            private void PerformAIAction(Character attacker, Character target)
            {
                // logic D'IA tour par tour
                int action = random.Next(1, 3); // 1 for attack, 2 for magic (no item implementation for now)

                if (action == 1)
                {
                    int attackIndex = random.Next(attacker.Attacks.Count);
                    PerformAttack(attacker, target);
                }
                else
                {
                    int magicIndex = random.Next(attacker.Magic.Count);
                    PerformMagic(attacker, target);
                }
            }

            private int CalculateDamage(int attack, int defense)
            {
                int damage = Math.Max(attack - defense, 0);
                return damage;
            }

            private int CalculateHealing(int healingPower)
            {
                int healingAmount = healingPower / 2; // Adjust as needed
                return healingAmount;
            }
        }

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

            // Créez des personnages de joueur et d'ennemi pour la démo
            Character player = new Character("Player", CharacterType.Warrior);
            Character enemy = new Character("Wild Pokemon", CharacterType.Mage);

            Battle battle = new Battle(player, enemy);

            // Réagir en fonction du choix du joueur
            switch (choice)
            {
                case 1:
                    // Attaquer
                    battle.PerformAttack(player, enemy);
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