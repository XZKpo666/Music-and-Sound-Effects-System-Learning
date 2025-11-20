using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


[System.Serializable]
public class KeyBoardRebindData
{
    public RemapKeyButton RebindButton;
    public InputActionReference Action;
    public int BindingIndex;
    public Text KeyDisplayText;
}
