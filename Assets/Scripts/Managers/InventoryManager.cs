using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{

    public ManagerStatus status { get; private set; }
    public string equippedItem { get; private set; }

    private Dictionary<string, int> _items;

    private NetworkService _network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Inventory manager starting...");

        _network = service;

        UpdateData(new Dictionary<string, int>());

        status = ManagerStatus.Started;
    }
    public void AddItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name] = _items[name] + 1;
        }
        else
        {
            _items[name] = 1;
        }

        // DisplayItems();
    }

    public bool ConsumeItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name]--;

            if ("health" == name)
            {
                Managers.Player.ChangeHealth(25);
            }

            if (0 == _items[name])
            {
                _items.Remove(name);
            }
        }
        else
        {
            Debug.Log("Cannot consume " + name);
            return false;
        }

        // DisplayItems();

        return true;
    }

    public List<string> GetItemList()
    {
        List<string> list = new List<string>(_items.Keys);
        return list;
    }

    public int GetItemCount(string name)
    {
        if (_items.ContainsKey(name))
        {
            return _items[name];
        }

        return 0;
    }

    public bool EquipItem(string name)
    {
        if (_items.ContainsKey(name) && equippedItem != name)
        {
            equippedItem = name;
            // Debug.Log("Equipped " + name);
            return true;
        }
        else
        {
            equippedItem = null;
            // Debug.Log("Unequipped");
            return false;
        }
    }

    private void DisplayItems()
    {
        string itemDisplay = "Items: ";

        foreach (KeyValuePair<string, int> item in _items)
        {
            itemDisplay = itemDisplay + item.Key + "(" + item.Value + ") ";
        }

        Debug.Log(itemDisplay);
    }

    public void UpdateData(Dictionary<string, int> items)
    {
        _items = items;
    }

    public Dictionary<string, int> GetData()
    {
        return _items;
    }
}
