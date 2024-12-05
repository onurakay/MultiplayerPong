using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviourPun, IPunObservable
{
    public int playerNumber;
    public float speed = 10f;
    public float boundaryY = 4.5f;

    private Rigidbody2D rb;
    private Vector2 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
        }
    }

    void InitializeRigidbody()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void HandleMovement()
    {
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            vertical = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            vertical = -1f;
        }

        float newY = transform.position.y + vertical * speed * Time.fixedDeltaTime;
        newY = Mathf.Clamp(newY, -boundaryY, boundaryY);

        rb.MovePosition(new Vector2(transform.position.x, newY));
    }

    public void ResetPaddlePosition()
    {
        transform.position = initialPosition;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (rb == null)
        {
            InitializeRigidbody();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D is still null during OnPhotonSerializeView.");
                return;
            }
        }

        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
        }
        else
        {
            object receivedData = stream.ReceiveNext();
            if (receivedData is Vector2 receivedPosition)
            {
                rb.position = Vector2.Lerp(rb.position, receivedPosition, Time.deltaTime * 10);
            }
            else
            {
                Debug.LogError("Invalid data type received for Paddle Position. Expected Vector2.");
            }
        }
    }
}
