using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool Dynamic = false;
    [SerializeField] private Transform lookAt = null;
    [SerializeField] private Vector3 offset = new Vector3(3.85f, 2.55f, -3.1f);

    private int desiredLane = 1;
    private GameController gameController;
    private Quaternion initalRotation;


    private void Start()
    {
        gameController = GameController.Instance;
        transform.position = lookAt.position + offset;
        initalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        LookAtPlayer();
        if (!gameController.isGameStarted) return;
        SwipeDetection();
        ApplyRotation();
    }

    private void SwipeDetection()
    {
        if (MobileInput.Instance.SwipeLeft)
        {
            MoveLane(false);
        }
        if (MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 desiredPosition = lookAt.position + offset;
        if (!Dynamic)
        {
            desiredPosition.x = 6.85f;
        }
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f);
    }

    private void ApplyRotation()
    {
        if (desiredLane == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(20, -20, 0), 0.05f);
        }
        else if (desiredLane == 2)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(20, -45, 0), 0.05f);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(20, -35, 0), 0.05f);
        }
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }
}
