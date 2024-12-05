using Photon.Pun;
using UnityEngine;

public class BallController : MonoBehaviourPun, IPunObservable
{
    public float speed = 8f;

    private Rigidbody2D rb;

    // variables for dead reckoning
    private Vector2 lastReceivedPosition;
    private Vector2 lastReceivedVelocity;
    private float timeSinceLastUpdate;

    void Start()
    {
        InitializeRigidbody();

        if (PhotonNetwork.IsMasterClient)
        {
            LaunchBall();
        }
    }

    void Update()
    {
        // dead reckoning and smoothing for non-master clients
        if (!photonView.IsMine)
        {
            PerformDeadReckoning();
        }
    }

    void InitializeRigidbody()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void LaunchBall()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(x, y).normalized;
        rb.velocity = direction * speed;
    }

    public void ResetBall()
    {
        rb.position = Vector2.zero;
        rb.velocity = Vector2.zero;

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke(nameof(LaunchBall), 1f);
        }
    }

    void PerformDeadReckoning()
    {
        timeSinceLastUpdate += Time.deltaTime;
        Vector2 predictedPosition = lastReceivedPosition + lastReceivedVelocity * timeSinceLastUpdate;

        // predicted position
        rb.position = Vector2.Lerp(rb.position, predictedPosition, Time.deltaTime * 10);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (rb == null)
        {
            InitializeRigidbody();
        }

        if (stream.IsWriting)
        {
            // Master client sends position and velocity
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            lastReceivedPosition = (Vector2)stream.ReceiveNext();
            lastReceivedVelocity = (Vector2)stream.ReceiveNext();
            timeSinceLastUpdate = 0f;
        }
    }
}
