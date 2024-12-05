using Photon.Pun;
using TMPro;

public class ScoreManager : MonoBehaviourPun
{
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    private int player1Score = 0;
    private int player2Score = 0;

    public void PlayerScored(int playerNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPC_UpdateScore", RpcTarget.All, playerNumber);
            ResetPositions();
        }
    }

    [PunRPC]
    void RPC_UpdateScore(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Score++;
        }
        else
        {
            player2Score++;
        }

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    public void ResetPositions()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPC_ResetPositions", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_ResetPositions()
    {
        BallController ball = FindObjectOfType<BallController>();
        ball.ResetBall();

        PaddleController[] paddles = FindObjectsOfType<PaddleController>();
        foreach (var paddle in paddles)
        {
            paddle.ResetPaddlePosition();
        }
    }
}
