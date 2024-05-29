using UnityEngine;

[CreateAssetMenu(fileName ="New Tool", menuName ="New Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    public int ItemDamge;
    public int ItemDuration;

    public override Consumable GetConsumable()
    {
        return null;
    }

    public override ItemClass GetItem()
    {
        return this;
    }

    public override MiscClass GetMiss()
    {
        return null;
    }

    public override ToolClass GetTool()
    {
        return this;
    }
}
