
using System;

public class Apple : IInventoryItem
{
    public IInventoryItemInfo info { get; }

    public IInventoryItemState state { get; }

    public Type type => GetType();

    public Apple(IInventoryItemInfo info)
    {
        this.info = info;
        state = new InventoryItemState();
    }

    public IInventoryItem Clone()
    {
        var clonedApple = new Apple(info);
        clonedApple.state.amount = state.amount;
        return clonedApple;
    }
}

