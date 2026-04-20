using System;

[Serializable]
public class ResAmountPair
{
    public ResourcesPanel.resource res;
    public int amount;

    public ResAmountPair(ResourcesPanel.resource res, int amount)
    {
        this.res = res;
        this.amount = amount;
    }
}