using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class ArithmeticDictionary : IDictionary<string, float>
{
    public ArithmeticDictionary(IDictionary<string, float> data, IEnumerable<string> fields = null, float? defaultValue = null)
    {
        _data = data;

        if (fields != null)
        {
            foreach (string f in fields)
            {
                if (!_data.ContainsKey(f))
                    _data[f] = defaultValue.GetValueOrDefault(0f);
            }
        }
    }

    public ArithmeticDictionary(IDictionary<string, float> data, IDictionary<string, float> defaultValues)
    {
        _data = data;

        foreach (var kvp in defaultValues)
        {
            if (!data.ContainsKey(kvp.Key))
                _data[kvp.Key] = kvp.Value;
        }
    }

    public ArithmeticDictionary(IEnumerable<string> fields, float defaultValue)
    {
        _data = new Dictionary<string, float>();
        foreach (string f in fields)
        {
            _data[f] = defaultValue;
        }
    }


    protected readonly IDictionary<string, float> _data;


    public ArithmeticDictionary One => CreateFromUnaryOperation(this, (x) => 1f);
    public ArithmeticDictionary Zero => CreateFromUnaryOperation(this, (x) => 0f);

    public ICollection<string> Keys => _data.Keys;

    public ICollection<float> Values => _data.Values;

    public int Count => _data.Count;

    public bool IsReadOnly => _data.IsReadOnly;

    public float this[string key] { get => _data[key]; set => _data[key] = value; }


    #region Utils methods

    public static ArithmeticDictionary CreateFromBinaryOperation(ArithmeticDictionary a, ArithmeticDictionary b, Func<float, float, float> action)
    {
        var newData = new Dictionary<string, float>();
        foreach (var key in a._data.Keys)
        {
            newData[key] = action.Invoke(a._data[key], b._data[key]);
        }
        return new ArithmeticDictionary(newData);
    }

    public static ArithmeticDictionary CreateFromUnaryOperation(ArithmeticDictionary a,  Func<float, float> action)
    {
        var newData = new Dictionary<string, float>();
        foreach (var key in a._data.Keys)
        {
            newData[key] = action.Invoke(a._data[key]);
        }
        return new ArithmeticDictionary(newData);
    }

    #endregion

    #region Math operations

    public static ArithmeticDictionary operator +(ArithmeticDictionary a, ArithmeticDictionary b)
    {
        return CreateFromBinaryOperation(a, b, (x,y) => x + y);
    }

    public static ArithmeticDictionary operator -(ArithmeticDictionary a, ArithmeticDictionary b)
    {
        return a + (-b);
    }


    public static ArithmeticDictionary operator *(ArithmeticDictionary a, ArithmeticDictionary b)
    {
        return CreateFromBinaryOperation(a, b, (x,y) => x * y);
    }


    public static ArithmeticDictionary operator *(ArithmeticDictionary a, float b)
    {
        return CreateFromUnaryOperation(a, (x) => x*b);
    }
    public static ArithmeticDictionary operator *(float b, ArithmeticDictionary a) => a * b;

    public static ArithmeticDictionary operator -(ArithmeticDictionary a)
    {
        return CreateFromUnaryOperation(a, (x) => -x);
    }

    public static ArithmeticDictionary operator -(float b, ArithmeticDictionary a)
    {
        return (a.One * b) - a;
    }

    public ArithmeticDictionary Power(ArithmeticDictionary pow)
    {
        return CreateFromBinaryOperation(this, pow, (x,y) => Mathf.Pow(x,y));
    }

    #endregion


    #region Dictionary interface methods
    public bool ContainsKey(string key)
    {
        return _data.ContainsKey(key);
    }

    public void Add(string key, float value)
    {
        _data.Add(key, value);
    }

    public bool Remove(string key)
    {
        return _data.Remove(key);
    }

    public bool TryGetValue(string key, out float value)
    {
        return _data.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<string, float> item)
    {
        _data.Add(item);
    }

    public void Clear()
    {
        _data.Clear();
    }

    public bool Contains(KeyValuePair<string, float> item)
    {
        return _data.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, float>[] array, int arrayIndex)
    {
        _data.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, float> item)
    {
        return _data.Remove(item);
    }

    public IEnumerator<KeyValuePair<string, float>> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_data).GetEnumerator();
    }

    #endregion
}