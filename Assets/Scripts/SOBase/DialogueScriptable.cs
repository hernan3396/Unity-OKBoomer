using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptable", menuName = "OKBoomer/New Dialogue", order = 0)]
public class DialogueScriptable : ScriptableObject
{
    [TextArea(10, 30)] public string Text;
}