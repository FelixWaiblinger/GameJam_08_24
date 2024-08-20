using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Data/Inventory")]
public class Inventory : ScriptableObject
{
    public int Size;
    public List<ItemType> Items = new();

    public bool Add(ItemType item)
    {
        if (Items.Count >= Size) return false;
        Items.Add(item);
        return true;
    }

    public void Remove(List<int> items)
    {
        items.Sort();
        items.Reverse();

        for (int i = 0; i < items.Count; i++)
        {
            Items.RemoveAt(items[i]);
        }
    }
}
