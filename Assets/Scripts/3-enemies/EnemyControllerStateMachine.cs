using UnityEngine;

/**
 * This component patrols between given points, chases a given target object when it sees it, and rotates from time to time.
 */
[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(Chaser))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Wanderer))]
public class EnemyControllerStateMachine: StateMachine {
    [SerializeField] float radiusToWatch = 5f;
    [SerializeField] float probabilityToRotate = 0.2f;
    [SerializeField] float probabilityToStopRotating = 0.2f;
    [SerializeField] float probabilityToStartWanderer = 0.2f;
    [SerializeField] float probabilityToStopWanderer = 0.1f;

    private Chaser chaser;
    private Patroller patroller;
    private Rotator rotator;
    private Wanderer wanderer;

    private float DistanceToTarget() {
        return Vector3.Distance(transform.position, chaser.TargetObjectPosition());
    }

    private void Awake() {
        chaser = GetComponent<Chaser>();
        patroller = GetComponent<Patroller>();
        rotator = GetComponent<Rotator>();
        wanderer = GetComponent<Wanderer>();

        base
        .AddState(patroller)     // This would be the first active state.
        .AddState(chaser)
        .AddState(rotator)
        .AddState(wanderer)
        .AddTransition(patroller, () => DistanceToTarget()<=radiusToWatch,   chaser)
        .AddTransition(rotator,   () => DistanceToTarget()<=radiusToWatch,   chaser)
        .AddTransition(wanderer,  () => DistanceToTarget()<=radiusToWatch,   chaser)
        .AddTransition(chaser,    () => DistanceToTarget() > radiusToWatch,  patroller)
        .AddTransition(rotator,   () => Random.Range(0f, 1f) < probabilityToStopRotating * Time.deltaTime, patroller)
        .AddTransition(rotator,   () => Random.Range(0f, 1f) < probabilityToStartWanderer * Time.deltaTime, wanderer)
        .AddTransition(wanderer,  () => Random.Range(0f, 1f) < probabilityToStopWanderer * Time.deltaTime, rotator)
        .AddTransition(patroller, () => Random.Range(0f, 1f) < probabilityToRotate       * Time.deltaTime, rotator)
        .AddTransition(patroller, () => Random.Range(0f, 1f) < probabilityToStartWanderer * Time.deltaTime, wanderer)
        ;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusToWatch);
    }
}
 