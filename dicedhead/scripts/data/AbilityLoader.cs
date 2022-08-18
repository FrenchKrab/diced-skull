using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class AbilityLoader : Node
{
    private static AbilityLoader Singleton {get; set;}

    private readonly Dictionary<string, AbilityData> _abilities = new Dictionary<string, AbilityData>();


    public override void _Ready()
    {
        Singleton = this;
        LoadAbilitiesData();
    }

    private void LoadAbilitiesData()
    {
        var jsonFiles = FilesUtils.ListAllFiles(AbilityData.InternalJsonFilesLocation);
        foreach (var fileName in jsonFiles)
        {
            string id = fileName.Replace(".json", "");
            var dict = AbilityData.LoadDictFromInternalFile(id);
            string dataTypeName = dict["_class"].ToString();
            Type t = Type.GetType(dataTypeName);
            var ability = (AbilityData)Activator.CreateInstance(t, dict, id);
            _abilities[id] = ability;
        }
    }


    public static List<string>[] GetUnlockOrder()
    {
        return new List<string>[] {
            new List<string>() {"bonus_dice1", "malus_dice1"},
            new List<string>() {"bonus_bdice1", "malus_dice2"},
            new List<string>() {"bonus_sword1", "malus_sdice1"},
            new List<string>() {"bonus_scissors1", "malus_dice3"},
        };
    }

    public static int[] GetUnlockKillRequired()
    {
        return new int[]
        {
            0,
            1,
            2,
            3,
            999
        };
    }

    public static List<string> GetIds()
    {
        return Singleton._abilities.Keys.ToList();
    }

    public static AbilityData GetAbility(string id)
    {
        return Singleton._abilities[id];
    }
}