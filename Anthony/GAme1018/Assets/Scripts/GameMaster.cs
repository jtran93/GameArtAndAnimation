using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static void killPlayer(Player player)
    {
        Destroy(player.gameObject);
    }
}
