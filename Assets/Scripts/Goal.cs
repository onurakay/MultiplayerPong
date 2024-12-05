using Photon.Pun;
using UnityEngine;

public class Goal : MonoBehaviourPun
{
    public int playerNumber; // 1 or 2

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                scoreManager.PlayerScored(playerNumber);
            }
        }
    }
}
