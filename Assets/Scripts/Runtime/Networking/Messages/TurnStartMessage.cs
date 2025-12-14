namespace CardGame
{
    [System.Serializable]
    public class TurnStartMessage : NetworkMessage
    {
        public string playerId;
    }
}
