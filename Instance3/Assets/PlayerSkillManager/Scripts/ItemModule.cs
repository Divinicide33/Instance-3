using UnityEngine;

public abstract class ItemModule : PlayerModule
{
    protected abstract void Use();
}

public enum ItemsName
{
    Potion,
    Arc

}