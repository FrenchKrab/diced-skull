using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

public abstract class AbilityData
{
    public const string InternalJsonFilesLocation = "res://dicedhead/data/abilities/";
    private const string ParentKey = "_parent";
    public const string ClassKey = "_class";
    private const string AbilityIconLocation = "res://dicedhead/sprites/abilities/";

    public AbilityData(IDictionary data, string id)
    {
        _internalData = data;
        Id = id;
    }

    public readonly IDictionary _internalData;

    public readonly string Id;
    public string Name => Get<string>("name", "??? ability ???");
    public string IconName => Get<string>("icon");
    public string ClassName => Get<string>(ClassKey);
    public Type Class => Type.GetType(ClassName);
    public Texture IconTexture => ResourceLoader.Load<Texture>($"{AbilityIconLocation}/{IconName}.png");
    public float IntensityValue => Get<float>("intensity", 0f);
    public string NodeResourceName => Get<string>("resource");
    public PackedScene NodeResourcePacked => ResourceLoader.Load<PackedScene>(NodeResourceName);


    public abstract float GetEstimatedAttack(IAbilityScalingFactors state);



    protected T Get<T>(string key)
    {
        return (T)_internalData[key];
    }

    protected T Get<T>(string key, T defaultValue)
    {
        if (_internalData.Contains(key))
            return (T)_internalData[key];
        else
            return defaultValue;
    }

    protected ArithmeticDictionary GetArithDict(string key)
    {
        var dict = (Godot.Collections.Dictionary)_internalData[key];
        var typedDict = dict.Keys.Cast<string>().ToDictionary(k=>k, k => (float)dict[k]);
        return new ArithmeticDictionary(typedDict);
    }

    protected ArithmeticDictionary GetArithDict(string key, ArithmeticDictionary defaultValue)
    {
        if (_internalData.Contains(key))
        {
            var dict = (Godot.Collections.Dictionary)_internalData[key];
            var typedDict = dict.Keys.Cast<string>().ToDictionary(k=>k, k => (float)dict[k]);
            return new ArithmeticDictionary(typedDict, defaultValue);
        }
        else
            return defaultValue;
    }


    public static IDictionary LoadDictFromJsonString(string json)
    {
        var parseResult = JSON.Parse(json);
        var result = parseResult.Result;
        if (!(result is Godot.Collections.Dictionary dict))
            throw new ArgumentException("json is not a dictionary");

        if (dict.Contains(ParentKey))
        {
            var dict2 = LoadDictFromInternalFile((string)dict[ParentKey]);
            foreach (var key in dict2.Keys)
            {
                if (!dict.Contains(key))
                    dict[key] = dict2[key];
            }
        }

        return dict;
    }

    public static IDictionary LoadDictFromInternalFile(string id)
    {
        var file = new File();
        var err = file.Open($"{InternalJsonFilesLocation}/{id}.json", File.ModeFlags.Read);
        return LoadDictFromJsonString(file.GetAsText());
    }
}