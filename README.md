# Multiplayer Pong Game

---

screenshot

---

##

- Two-player game using Photon PUN, ensuring real-time synchronization between players.
- Implements dead reckoning for smooth ball and paddle movements, compensating for network latency.
- Tracks scores for both players, with updates displayed dynamically on the UI.
- Classic Pong game mechanics.

##
- Predictive positioning for clients not hosting the game (**Dead Reckoning**).
- Automatic re-launch after scoring.
- Paddle positions reset after scoring.

- Triggers scoring events when the ball enters the opposing goal area.

## Future Plans

- Advanced Lobbies and Room Management
- Global or local leaderboards for high scores.
- Sound Effects
- Visual Enhancements
- Improving UI

---

## Code Highlights

### Dead Reckoning
Dead reckoning reduces visual lag for non-host clients:
```csharp
void PerformDeadReckoning()
{
    timeSinceLastUpdate += Time.deltaTime;
    float latencyCompensation = PhotonNetwork.GetPing() * 0.001f;
    Vector2 predictedPosition = lastReceivedPosition + lastReceivedVelocity * (timeSinceLastUpdate + latencyCompensation);
    rb.position = Vector2.Lerp(rb.position, predictedPosition, Time.deltaTime * 10);
}
```

### Paddle Movement
Smooth and clamped paddle movement:
```csharp
private void HandleMovement()
{
    float vertical = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? -1f : 0f;
    float newY = Mathf.Clamp(transform.position.y + vertical * speed * Time.fixedDeltaTime, -boundaryY, boundaryY);
    rb.MovePosition(new Vector2(transform.position.x, newY));
}
```

### Photon Integration
Efficient data synchronization and event handling:
```csharp
public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
{
    if (stream.IsWriting)
    {
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
```
