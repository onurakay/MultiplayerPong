using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private GameObject ballPrefab;

    void Awake()
    {
        OptimizePhotonSettings();
    }

    void Start()
    {
        SetScreenResolution();

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("spawning thingss");
            SpawnPaddle();
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnBall();
            }
        }
    }

    static void OptimizePhotonSettings()
    {
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 15;
    }

        private static void SetScreenResolution()
    {
        int width = 960;
        int height = 540;
        bool isFullScreen = false;

        Screen.SetResolution(width, height, isFullScreen);
    }

    void SpawnPaddle()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        float xPosition = (playerNumber == 1) ? -7f : 7f;

        GameObject paddle = PhotonNetwork.Instantiate(paddlePrefab.name, new Vector2(xPosition, 0), Quaternion.identity);
        paddle.name = $"Paddle_{playerNumber}";
        paddle.GetComponent<PaddleController>().playerNumber = playerNumber;
    }

    void SpawnBall()
    {
        PhotonNetwork.Instantiate(ballPrefab.name, Vector2.zero, Quaternion.identity);
    }
}