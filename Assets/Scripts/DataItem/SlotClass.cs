using System;
using UnityEngine;

[Serializable]
public class SlotClass
{
    public ItemClass item;
    public int quantity;

    public SlotClass(ItemClass _itemClass, int _quantity){
        this.item = _itemClass;
        this.quantity = _quantity;
    }

    public SlotClass(){}

    public ItemClass GetItem(){
        return this.item;
    }

    public int GetQuantity(){
        return this.quantity;
    }

    public void PlusQuantity(int _quantity){
        this.quantity += _quantity;
    }

    public void SubQuantity(int _quantity){
        this.quantity -= _quantity;
    }
}