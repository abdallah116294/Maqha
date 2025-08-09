using Maqha.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Maqha.Repository.Data
{
    public static class MaqhaContextSeed
    {
        public static async Task SeedAsync(MaqhaDbContext context)
        {
            try
            {
                //Seed Cafe Info 
                await SeedCafeInfo(context);
                //Seed MenuItem 
                await SeedMenuItme(context);

            }
           
            catch (Exception ex)
            {

                Console.WriteLine($"Error during seeding: {ex.Message}");
                throw;
            }
        }
        public static async Task SeedCafeInfo(MaqhaDbContext context)
        {
            try
            {
                if (!context.CafeInfos.Any())
                {
                    // Read the JSON file
                    var cafeInfosData = await File.ReadAllTextAsync("../Maqha.Repository/Data/DataSeed/CafeInfo.json");

                    // Deserialize the JSON data
                    var cafeInfos = JsonSerializer.Deserialize<IEnumerable<CafeInfo>>(cafeInfosData);

                    if (cafeInfos != null && cafeInfos.Any())
                    {
                        // Add the data to the context
                        await context.CafeInfos.AddRangeAsync(cafeInfos);

                        // Save changes to the database
                        await context.SaveChangesAsync();

                        Console.WriteLine($"Successfully seeded {cafeInfos.Count()} CafeInfo records.");
                    }
                    else
                    {
                        Console.WriteLine("No CafeInfo data found in JSON file.");
                    }
                }
                else
                {
                    Console.WriteLine("CafeInfo data already exists. Skipping seeding.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("CafeInfo.json file not found. Please check the file path.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }
        }
        public static async Task SeedMenuItme(MaqhaDbContext context)
        {
            try
            {
                //Seed MenuItme
                if (!context.MenuItems.Any())
                {
                    //Read JSON File 
                    var menuItemJsonFile = await File.ReadAllTextAsync("../Maqha.Repository/Data/DataSeed/menuItem.json");
                    // Deserialize the JSON data
                    var menutItems = JsonSerializer.Deserialize<IEnumerable<MenuItem>>(menuItemJsonFile);
                    if (menutItems != null && menutItems.Any())
                    {
                        // Add the data to the context
                        await context.MenuItems.AddRangeAsync(menutItems);

                        // Save changes to the database
                        await context.SaveChangesAsync();

                        Console.WriteLine($"Successfully seeded {menutItems.Count()} MenuItem records.");
                    }
                    else
                    {
                        Console.WriteLine("No MenuItem data found in JSON file.");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("MenuItem.json file not found. Please check the file path.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }
        }
    }
         
}
