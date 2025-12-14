using UnityEngine;

namespace CardGame
{
    public static class LocalPlayerContext
    {
        public static PlayerSlot Slot { get; private set; } = PlayerSlot.None;
        public static ulong ClientId { get; private set; }

        public static bool IsPlayer1 => Slot == PlayerSlot.Player1;
        public static bool IsPlayer2 => Slot == PlayerSlot.Player2;

        public static void Set(PlayerSlot slot, ulong clientId)
        {
            Slot = slot;
            ClientId = clientId;

            Debug.Log($"[LocalPlayerContext] I am {slot} (ClientId: {clientId})");
        }
    }
}
