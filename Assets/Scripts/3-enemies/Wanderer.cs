using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Wanderer : TargetMover {
    [Tooltip("The maximum distance the wanderer will move each time.")]
    [SerializeField] float maxMovementDistanceX = 8f;
    [SerializeField] float maxMovementDistanceY = 4f;

    [Tooltip("How often the wanderer will change direction (in seconds).")]
    [SerializeField] float changeDirectionInterval = 6f;
    

    private Vector3 randomTarget;
    private float timer;

    protected override void Start() {
        base.Start();
        SetRandomTarget(); // Set the first target
        timer = changeDirectionInterval;
    }

    private void Update() {
        // Update timer to change direction
        timer -= Time.deltaTime;

        if (timer <= 0) {
            // Reset timer and choose a new random target position
            timer = changeDirectionInterval;
            SetRandomTarget();
        }

        // The target position changes periodically, but the movement behavior is handled by TargetMover
    }

    private void SetRandomTarget() {
        // Randomly generate a target position within a defined range from the current position

        Vector3 randomDirection = new Vector3(Random.Range(-maxMovementDistanceX-transform.position.x, maxMovementDistanceX-transform.position.x), 
                                              Random.Range(-maxMovementDistanceY-transform.position.y, maxMovementDistanceY-transform.position.y), 
                                              0);
        randomTarget = transform.position + randomDirection;
        
        SetTarget(randomTarget);  // Update the target for the TargetMover to follow
    }
}
