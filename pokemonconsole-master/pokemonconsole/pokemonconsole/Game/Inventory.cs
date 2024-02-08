// InventoryManager.cs
using System;

class InventoryManager
{
    public static void DisplayInventory(string[] inventory)
    {
        ConsoleKeyInfo key;
        int pointerPos = 1;

        do
        {
            Console.Clear();
            // Display the inventory with the cursor highlighting the selected slot
            for (int i = 0; i < inventory.Length; i++)
            {
                if (i == pointerPos)
                {
                    // Change color for the four lines below the selected slot
                    for (int j = 0; j <= 3 && i + j < inventory.Length; j++)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(inventory[i + j]);
                        Console.ResetColor();
                    }

                    // Skip the next four lines
                    i += 3;
                }
                else
                {
                    Console.WriteLine(inventory[i]);
                }
            }

            key = Console.ReadKey(true); // Wait for user input

            // Update the cursor position based on user input
            switch (key.Key)
            {
                case ConsoleKey.Z:
                    pointerPos = Math.Max(1, pointerPos - 4); // Move the cursor up
                    break;
                case ConsoleKey.S:
                    pointerPos = Math.Min(inventory.Length - 5, pointerPos + 4); // Move the cursor down
                    break;
            }
        } while (key.Key != ConsoleKey.I); // Exit the loop if the I key is pressed
        Console.Clear();
    }
}
