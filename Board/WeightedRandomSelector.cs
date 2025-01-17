using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Generic weighted random selection.
/// You can add entries with a weight, then GetRandomEntry() returns an item
/// with probability proportional to its weight.
/// </summary>
[System.Serializable]
public class WeightedRandomSelector<T>
{
    [System.Serializable]
    public class Entry
    {
        public T item;
        public float weight;
        public Entry(T item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }
    }

    private List<Entry> entries = new List<Entry>();
    private float totalWeight = 0f;
    private System.Random rng = new System.Random();

    public void AddEntry(T item, float weight)
    {
        entries.Add(new Entry(item, weight));
        totalWeight += weight;
    }

    public T GetRandomEntry()
    {
        if (entries.Count == 0)
        {
            throw new InvalidOperationException("No entries in WeightedRandomSelector");
        }
        float r = (float)(rng.NextDouble() * totalWeight);
        float cumulative = 0f;
        for (int i = 0; i < entries.Count; i++)
        {
            cumulative += entries[i].weight;
            if (r <= cumulative)
            {
                return entries[i].item;
            }
        }
        return entries[entries.Count - 1].item; // fallback
    }
}
