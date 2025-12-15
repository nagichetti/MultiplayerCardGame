using CardGame;
using UnityEngine;

public static class LocalPlayerContext
{
    public static string PlayerId { get; private set; }
    public static PlayerSlot MySlot { get; private set; }

    public static void Init()
    {
        if (PlayerPrefs.HasKey("PLAYER_ID"))
            PlayerId = PlayerPrefs.GetString("PLAYER_ID");
        else
        {
            PlayerId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PLAYER_ID", PlayerId);
        }
    }

    public static void SetSlot(PlayerSlot slot)
    {
        MySlot = slot;
    }
}