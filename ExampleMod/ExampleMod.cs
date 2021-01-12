using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using MonoMod;
using UnityEngine;

namespace ExampleMod
{
    // You use BepInDependency to mark a mod that your mod requires to work. This is not required if you don't have dependencies.
    [BepInDependency("evaisa.MonSancAPI")]
    // BepInPlugin is required to make BepInEx properly load your mod, this tells BepInEx the ID, Name and Version of your mod.
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ExampleMod : BaseUnityPlugin
    {
        // Some constants holding the stuff we put in BepInPlugin, we just made these seperate variables so that we can more easily read them.
        public const string ModGUID = "evaisa.examplemod";
        public const string ModName = "Example Mod";
        public const string ModVersion = "0.1.0";

        // Create a ConfigEntry so we can reference our config option.
        private ConfigEntry<bool> ExampleConfigOption;

        // This is the first code that runs when your mod gets loaded.
        public ExampleMod()
        {
            // Binding a config option, making it actually be registered by BepInEx.
            ExampleConfigOption = Config.Bind(
                "General", // The category in the config file.
                "ExampleOption", // The name of the option
                true, // The default value
                "This is a example option." // The description
            );

            // To modify game functions you can use monomod.
            // This routes the OpenChest function into our function.
            On.Chest.OpenChest += Chest_OpenChest; 
        }

        private void Chest_OpenChest(On.Chest.orig_OpenChest orig, Chest self)
        {
            // If our config options is true run the code inside.
            if (ExampleConfigOption.Value)
            {

                // Get the Manticorb egg item from the game's referenceable prefabs list.
                var manticorb_egg = GameController.Instance.WorldData.Referenceables.FirstOrDefault(item =>
                {
                    if (item != null)
                    {
                        if (item.gameObject.name == "ManticorbEgg")
                        {
                            return true;
                        }
                    }

                    return false;
                });

                // Set the chest's item to be the manticorb egg prefab.
                if (manticorb_egg != null)
                {
                    self.Item = manticorb_egg.gameObject;
                }
            }

            // Run the original function.
            // If you don't run the original function you will basically empty out the function, you can use this to overwrite game functions.
            // You can run your code after or before you call the original function depending on when you want to run your code.
            // If a function requires a return value you can do "return orig(self)"
            // If a function has multiple argument you'd do "orig(self, argument1, argument2)"
            orig(self);
        }

        // You can also have basic unity callbacks like any other MonoBehaviour.
        void Update()
        {

        }
    }
}
