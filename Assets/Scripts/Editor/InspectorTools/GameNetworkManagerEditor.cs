using UnityEditor;
using UnityEngine;

namespace CardGame.Editor
{
    [CustomEditor(typeof(GameNetworkManager))]
    public class GameNetworkManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();
            GameNetworkManager manager = (GameNetworkManager)target;
            if (GUILayout.Button("Host"))
            {
                manager.StartServer();
            }
            if (GUILayout.Button("Join"))
            {
                manager.StartCilent();
            }
        }
    }
}
