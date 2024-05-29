using UnityEngine;

[CreateAssetMenu(fileName ="New Misc", menuName ="New Item/Misc")]
public class MiscClass : ItemClass
{
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
        return this;
    }

    public override ToolClass GetTool()
    {
        return null;
    }
}
