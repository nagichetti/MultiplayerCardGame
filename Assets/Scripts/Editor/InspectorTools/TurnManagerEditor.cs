using UnityEditor;
using UnityEngine;

namespace CardGame.Editor
{
    [CustomEditor(typeof(TurnManager))]
    public class TurnManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TurnManager manager = (TurnManager)target;
            if(GUILayout.Button("End Turn"))
            {
                manager.SendEndTurnMsg(manager.currentplayerTurn);
            }
        }
    }
}
