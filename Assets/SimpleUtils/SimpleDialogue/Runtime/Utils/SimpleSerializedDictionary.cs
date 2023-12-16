using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.Utils
{
    [Serializable]
    public class SimpleSerializedDictionary<TValue> : ISerializationCallbackReceiver where TValue : ISerializedDictionaryValue
    {
        [SerializeField] private List<int> keys = new();
        [SerializeField] private List<TValue> values = new();

        private Dictionary<int, TValue> _dictionary = new();


        public TValue GetValueAtIndex(int i)
        {
            return values[i];
        }

        public TValue this[int key]
        {
            get => _dictionary[key];
            set
            {
                keys.Add(key);
                values.Add(value);
                _dictionary.Add(key, value);
            }
        }

        public bool TryGetValue(int index, out TValue value)
        {
            return _dictionary.TryGetValue(index, out value);
        }

        public List<TValue> GetValues()
        {
            return _dictionary.Values.ToList();
        }

        public void Add(TValue value)
        {
            _dictionary[value.ID] = value;
        }

        public void Remove(TValue value)
        {
            _dictionary.Remove(value.ID);
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
            _dictionary.Clear();
        }
        
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach(var pair in _dictionary)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _dictionary.Clear();

            if(keys.Count != values.Count)
                throw new IndexOutOfRangeException();

            for(int i = 0; i < keys.Count; i++)
                _dictionary.Add(keys[i], values[i]);
        }
    }
}