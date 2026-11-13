using UnityEngine;

public class PlayerModelAnimator : MonoBehaviour
{
    public Transform player;      // drag Player here
    public Animator animator;     // drag the astronaut's Animator here
    public float maxStep = 1.5f;  // bigger jumps are respawns, so ignore them
    public float stopDelay = 0.1f; // must be still this long before a new jump can start

    Vector3 lastPos;
    bool moving;
    float stillTimer;

    void Start() { lastPos = player.position;
        
     }

    void Update()
    {
        Vector3 delta = player.position - lastPos;
        delta.y = 0;
        bool movedThisFrame = delta.sqrMagnitude > 0.00001f && delta.magnitude < maxStep;

        if (movedThisFrame)
        {
            stillTimer = 0f;
            if (!moving)
            {
                moving = true;
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");
            }
            transform.rotation = Quaternion.LookRotation(delta);
        }
        else
        {
            stillTimer += Time.deltaTime;
            if (stillTimer >= stopDelay) moving = false;
        }

        lastPos = player.position;
    }
}