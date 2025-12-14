using System;
using UnityEngine;

namespace CardGame
{
    public class LocalPlayer : Player
    {
        [SerializeField]
        private PlayerSlot slot;
        [SerializeField]
        private GameData GameData;
        private void OnEnable()
        {
            slot = LocalPlayerContext.Slot;
        }
    }
}
