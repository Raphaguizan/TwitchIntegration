using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerCustonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Player myPlayer = (Player)target;
        DrawDefaultInspector();
        GUILayout.Space(20f);
        if (GUILayout.Button("Charge"))
            myPlayer.AddFill();

        if (GUILayout.Button("Run"))
            myPlayer.StartRun();

    }
}
#endif