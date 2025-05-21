using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public string flavor;

    public abstract void Use();
}