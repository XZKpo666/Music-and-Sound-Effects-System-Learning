using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


[System.Serializable]
public class RebindData
{
    public RebindButton RebindButton;
    public InputActionReference Action;
    public int BindingIndex;
    public Text KeyDisplayText;
}
