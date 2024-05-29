using UnityEngine;

[CreateAssetMenu(fileName ="New Comsumable", menuName ="New Item/Comsumable")]
public class Consumable : ItemClass
{
    [SerializeField] private float healRecovery;
    [SerializeField] private float foodRecovery;
    [SerializeField] private float healConsumable;
    public override Consumable GetConsumable()
    {
        return this;
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
        return null;
    }
}
