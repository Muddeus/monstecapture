using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class StateMachineFlee : MonoBehaviour, ITrappable
{
    public enum State
    {
        Patrol,
        Fleeing,
        Attack
    }
    public State state;

    public GameObject player;
    private Rigidbody rigidbody;
    private Vector3 originalScale;
    public float walkSpeed;
    public float runSpeed;

    public PlayerManager playerManager;

    [SerializeField] List<Transform> points;

    [SerializeField] MeshRenderer meshRen;

    [SerializeField] Material matPatrol;
    [SerializeField] Material matFleeing;
    [SerializeField] Material matAttack;

    public bool isBeingCaptured { get; set; }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        originalScale = Vector3.one;
        NextState();
    }

    void NextState()
    {
        switch (state)
        {
            case State.Patrol:
                StartCoroutine(PatrolState());
                break;
            case State.Fleeing:
                StartCoroutine(FleeingState());
                break;
            case State.Attack:
                StartCoroutine(AttackState());
                break;
        }
    }

    IEnumerator PatrolState()
    {
        Debug.Log("Entering Patrol State");
        int currentIndex = 0;
        while (state == State.Patrol)
        {
            meshRen.material = matPatrol;

            transform.position = Vector3.MoveTowards(transform.position, points[currentIndex].position, walkSpeed / 30);

            if ((transform.position - points[currentIndex].position).magnitude < 1f)
            {
                currentIndex++;
                currentIndex = currentIndex % points.Count;
            }

            yield return null;
        }
        Debug.Log("Exiting Patrol State");
        NextState();
    }

    IEnumerator FleeingState()
    {
        Debug.Log("Entering Fleeing State");
        while (state == State.Fleeing)
        {
            meshRen.material = matFleeing;

            Vector3 direction =  transform.position - player.transform.position;
            rigidbody.AddForce(direction.normalized * (1000f * Time.deltaTime));

            
            if (direction.magnitude < 3f)
            {
                state = State.Attack;
            }
            

            yield return null;
        }
        Debug.Log("Exiting Fleeing State");
        NextState();
    }

    IEnumerator AttackState()
    {
        Debug.Log("Entering Attack State");

        Vector3 direction = player.transform.position - transform.position;
        rigidbody.AddForce(direction.normalized * (75000f * Time.deltaTime));


        while (state == State.Attack)
        {
            meshRen.material = matAttack;

            yield return new WaitForSeconds(0.5f);
            state = State.Patrol;
        }

        Debug.Log("Exiting Attack State");
        NextState();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state != State.Attack) return;
        if (other.gameObject == player)
        {
            playerManager.TakeDamage();

            Rigidbody rb = player.GetComponent<Rigidbody>();

            Vector3 hitDir = player.transform.position - other.contacts[0].point;

            rb.AddForce(hitDir.normalized * 2000f);
        }
    }

    private float timer = 1;
    public bool CaptureAnimation()
    {
        isBeingCaptured = true;
        timer -= Time.deltaTime * 1f;
        transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, timer);

        if (timer <= 0)
        {
            return false;
        }

        return true;
    }

    public int PointValue()
    {
        return 2;
    }

    public void OnTriggerEnter(Collider other)
    {
        print("Trigger entered");
        if (other.CompareTag("Player"))
        {
            state = State.Fleeing;
            Debug.Log("player in flee zone");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            state = State.Patrol;
        }
    }
}
