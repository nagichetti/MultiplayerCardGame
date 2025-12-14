namespace CardGame
{
    [System.Serializable]
    public class PlayerAssignMessage : NetworkMessage
    {
        public string playerSlot;
    }
}
