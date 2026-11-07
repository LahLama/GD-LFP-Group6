using UnityEngine;

public class PlayerModelAnimator : MonoBehaviour
{
    public Transform player;     // drag Player here
    public Animator animator;    // drag the astronaut's Animator here
    public float maxStep = 1.5f; // bigger jumps are respawns, so ignore them

    Vector3 lastPos;
    bool wasMoving;

    void Start() { lastPos = player.position; }

    void Update()
    {
        Vector3 delta = player.position - lastPos;
        delta.y = 0;
        bool moving = delta.sqrMagnitude > 0.00001f && delta.magnitude < maxStep;

        if (moving && !wasMoving) animator.SetTrigger("Jump");
        if (moving) transform.rotation = Quaternion.LookRotation(delta);

        wasMoving = moving;
        lastPos = player.position;
    }
}
