using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketManager : ETGModule
{
    private Dictionary<string, string> knownFloors = new Dictionary<string, string>();

    private static string[] brokenFloors = new string[6] {
        "tt_belly",
        "tt_future",
        "tt_jungle",
        "tt_nakatomi",
        "tt_phobos",
        "tt_west"
    };

    private string[] ReturnMatchesFrom(string matchThis, string[] inThis)
    {
        List<string> stringList = new List<string>();
        foreach (string inThi in inThis)
        {
            if (StringAutocompletionExtensions.AutocompletionMatch(inThi, matchThis))
                stringList.Add(inThi);
        }
        return stringList.ToArray();
    }

    private bool isInArray(string[] array, string dungeon)
    {
        foreach (string d in array)
        {
            if (d == dungeon)
            {
                return true;
            }
        }
        return false;
    }

    public override void Init()
    {
        knownFloors.Add("tutorial", "tt_tutorial");
        knownFloors.Add("breach", "tt_foyer");
        knownFloors.Add("keep", "tt_castle");
        knownFloors.Add("gungeon_proper", "tt5");
        knownFloors.Add("mines", "tt_mines");
        knownFloors.Add("hollow", "tt_catacombs");
        knownFloors.Add("forge", "tt_forge");
        knownFloors.Add("bullet_hell", "tt_bullethell");
        knownFloors.Add("oubliette", "tt_sewer");
        knownFloors.Add("abbey", "tt_cathedral");
        knownFloors.Add("rat_den", "ss_resourcefulrat");
        knownFloors.Add("hunter_past", "fs_guide");
        knownFloors.Add("pilot_past", "fs_pilot");
        knownFloors.Add("convict_past", "fs_convict");
        knownFloors.Add("marine_past", "fs_soldier");
        knownFloors.Add("robot_past", "fs_robot");
        knownFloors.Add("bullet_past", "fs_bullet");
        knownFloors.Add("cultist_past", "fs_coop");
        knownFloors.Add("b_belly", "tt_belly");
        knownFloors.Add("b_future", "tt_future");
        knownFloors.Add("b_jungle", "tt_jungle");
        knownFloors.Add("b_nakatomi", "tt_nakatomi");
        knownFloors.Add("b_phobos", "tt_phobos");
        knownFloors.Add("b_west", "tt_west");
    }

    public override void Start()
    {
        ETGModConsole.Commands.AddUnit("load_level", new Action<string[]>(Teleport), new AutocompletionSettings((index, input) =>
        {
            if (index == 0)
                return ReturnMatchesFrom(input, knownFloors.Keys.ToArray());
            return new string[0];
        }));
    }

    public override void Exit()
    {
    }

    public virtual void Teleport(string[] args)
    {
        if (args.Length == 0 || args.Length != 0 && args[0] == "help")
        {
            ETGModConsole.Log("Original mod Anywhere by stellatedHexahedron, thanks to Zatherz, the MTG lead developer, for helping me out on this mod.\nload_level Usage: load_level <level>\nAttempting to load a level from foyer will start a \"corrupted run,\" as attempting to leave the run by teleporting back to breach will hardlock your game.\nAny level that starts with b_ is a broken level. Use at your own risk! To bypass the warning, pass -c as the second argument.");
            return;
        }
        else if (args.Length > 0 && args[0] == "dump")
        {
            ETGModConsole.Log("Attempting to dump floors to console log...");
            for (int index = 0; index < GameManager.Instance.dungeonFloors.Count; ++index)
            {
                ETGModConsole.Log("Logging: " + GameManager.Instance.dungeonFloors[index].dungeonSceneName);
            }
            for (int index = 0; index < GameManager.Instance.customFloors.Count; ++index)
            {
                ETGModConsole.Log("Logging: " + GameManager.Instance.customFloors[index].dungeonSceneName);
            }
            return;
        }
        else
        {
            try
            {
                ETGModConsole.Log("Attempting to load level \"" + args[0] + "\"");
                string dungeon = knownFloors[args[0]];
                if (dungeon != null)
                {
                    if (isInArray(brokenFloors, dungeon))
                    {
                        if (args.Length > 1 && args[1] == "-c")
                        {
                            GameManager.Instance.LoadCustomLevel(dungeon);
                            ETGModConsole.Log("Successfully loaded BROKEN level \"" + args[0] + "\"");
                        }
                        else
                        {
                            ETGModConsole.Log("Take a few steps back! Are you sure you would like to load that broken level? It may crash (or softlock) Gungeon and destroy your run progress!\nIf you are surely sure, pass -c after the level. e.g: load_level <b_level> -c");
                        }
                    }
                    else
                    {
                        GameManager.Instance.LoadCustomLevel(dungeon);
                        ETGModConsole.Log("Successfully loaded level \"" + args[0] + "\"");
                    }
                }
            }
            catch (Exception)
            {
                ETGModConsole.Log("WOAH there tiger! Are you trying to load a level that doesn't exist?");
            }
        }
    }
}