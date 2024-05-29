using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="New Item/Item")]
public abstract class ItemClass : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public float itemWeight;
    public abstract ItemClass GetItem();
    public abstract ToolClass GetTool();
    public abstract Consumable GetConsumable();
    public abstract MiscClass GetMiss();
}
