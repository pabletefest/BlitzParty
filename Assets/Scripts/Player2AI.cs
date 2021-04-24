using System;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class Player2AI : MonoBehaviour
{
    private bool isFacingRight = true; 
    private Vector3 m_Velocity = Vector3.zero;

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    private Animator animator;
    public Transform player2; //enemy itself
    public float speed = 200f;
    public float nextWaypointDistance = 3;
    public Transform Player2GFX;
    public Vector2 target;

    [SerializeField]
    private List<Transform> enemiesList;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private Collider2D player2Collider;

    private void OnEnable()
    {
        EnemySpawner.OnEnemySpawn += EnemySpawned;
        RabbitCapture.OnEnemyCaptured += EnemyCaptured;
        ResetRabbitPursuit.OnSceneRestarted += SceneRestarted;
    }

    private void OnDisable()
    {
        EnemySpawner.OnEnemySpawn -= EnemySpawned;
        RabbitCapture.OnEnemyCaptured -= EnemyCaptured;
        ResetRabbitPursuit.OnSceneRestarted -= SceneRestarted;
    }


    private void Awake()
    {
        enemiesList = new List<Transform>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        player2Collider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void SceneRestarted(string activeScene)
    {
        enemiesList.Clear();
    }

    private void EnemySpawned(GameObject enemy)
    {
        enemiesList.Add(enemy.transform);
        UpdatePath();
    }
    private void EnemyCaptured(GameObject enemy)
    {
        enemiesList.Remove(enemy.transform);
        UpdatePath();
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            Transform nearestEnemy = transform;
            float nearestDistance = 100f;

            foreach (Transform enemy in enemiesList)
            {
                if (enemy != null)
                {
                    if (Vector2.Distance(enemy.position, transform.position) < nearestDistance)
                    {
                        nearestEnemy = enemy;
                    }
                }
            }

            target.x = nearestEnemy.position.x;
            target.y = nearestEnemy.position.y;

            /*
            if(player.position.x > 0f)
            {
                target.x = Random.Range(-8.3f, 0f);
            }
            else
            {
                target.x = Random.Range(0f, 8.3f);
            }

            target.y = Random.Range(-4.4f, 3.3f);
            */
            seeker.StartPath(rb.position, target, OnPathComplete);
        }
            
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 directionRaw = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 velocity = direction * speed * Time.deltaTime;


        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocity, ref m_Velocity, m_MovementSmoothing);
        //rb.AddForce(force);


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (directionRaw.x > 0 && !isFacingRight)
		{
			Flip();
		}
		else if (directionRaw.x < 0 && isFacingRight)
		{
			Flip();
		}
        
        if (rb.velocity.x != 0f)
		{
			animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
		}
		else
		{
			animator.SetFloat("Speed", Mathf.Abs(rb.velocity.y));
		}
    }

    private void Flip()
	{
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    void OnCollision2DEnter(Collision2D other)
    {
      if (other.gameObject.tag == "Player")
      {
        Physics2D.IgnoreCollision(other.collider, player2Collider);
      }
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }
}
