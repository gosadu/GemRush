using System;

[System.Serializable]
public class ShopItem
{
    public string id;
    public string name;
    public int cost;
    public string type;  // "offense", "defense", "immunity"
    public int amount;   // how much it gives

    // Parameterless constructor if you want object initializers
    public ShopItem() { }

    public ShopItem(string id, string name, int cost, string type, int amount)
    {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.type = type;
        this.amount = amount;
    }
}
