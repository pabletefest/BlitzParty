using Mirror;
using Pathfinding;
using Services;
using UnityEngine;

namespace Online.BinkyPursuit
{
    public class EnemyAIOnline : NetworkBehaviour
    {

        public Transform player;
        public float speed = 200f;
        public float nextWaypointDistance = 3;
        public Transform opossumGFX;
        public Vector2 target;


        Path path;
        int currentWaypoint = 0;
        bool reachedEndOfPath = false;

        Seeker seeker;
        Rigidbody2D rb;

        [SerializeField]
        private Collider2D rabbitCollider;

        private IObjectPooler objectPooler;

        // Start is called before the first frame update
        void Start()
        {
            //if (!isServer) return;
            
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();

            //objectPooler = ServiceLocator.Instance.GetService<IObjectPooler>();

            InvokeRepeating("UpdatePath", 0f, 1.5f);
        }

        public override void OnStartClient()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
        }

        public override void OnStartServer()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
        }

        void UpdatePath()
        {
            if (seeker.IsDone())
            {
                target.x = Random.Range(-8.3f, 8.3f);
                target.y = Random.Range(-4.4f, 3.3f);

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
            if (!isServer) return;
            
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

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            RpcMoveEnemyOnClients(force);
            //rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if(distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            RpcSwapEnemySprite();
            /*if (rb.velocity.x >= 0.01f)
            {
                opossumGFX.localScale = new Vector3(-1f, 1f, 1f);

            }
            else if (rb.velocity.x <= -0.01f)
            {
                opossumGFX.localScale = new Vector3(1f, 1f, 1f);
            }*/
        }

        [ClientRpc]
        private void RpcMoveEnemyOnClients(Vector2 force)
        {
            if (rb) rb.AddForce(force);
        }

        [ClientRpc]
        private void RpcSwapEnemySprite()
        {
            if (rb.velocity.x >= 0.01f)
            {
                opossumGFX.localScale = new Vector3(-1f, 1f, 1f);

            }
            else if (rb.velocity.x <= -0.01f)
            {
                opossumGFX.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
