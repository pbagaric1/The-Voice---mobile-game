using UnityEngine;
using System.Collections;
using System;

public class Boss : MonoBehaviour, ITakeDamage 
{
    public static Boss Instance { get; private set; }

    public Transform[] spots;
    public float speed;
    public float jumpSpeed;
    public int MaxHealth = 100;
    public float projectileSpeed;
    private bool vulnerable;
    private bool dead;
    private CameraShake cameraShake;
    public int Health { get; private set; }

    public Transform[] shootSpots;
    public GameObject projectile;
    public GameObject Wall;
    public GameObject EndWall;
    public HealthBarBoss HealthBar;
    private Vector3 origWallPosition;
    private bool isJumping;

    private Player player;
    private WallTrigger wallTrigger;
    Vector3 playerPosition;
    public AudioClip SpawnProjectileSound;
    public GameObject DestroyedEffect;
    public Animator Animator;


    public void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        Health = MaxHealth;
        player = GameObject.FindObjectOfType(typeof(Player)) as Player;
        cameraShake = GameObject.FindObjectOfType(typeof(CameraShake)) as CameraShake;
        wallTrigger = GameObject.FindObjectOfType(typeof(WallTrigger)) as WallTrigger;
        HealthBar = FindObjectOfType<HealthBarBoss>();
        origWallPosition.x = Wall.transform.position.x;
        origWallPosition.y = Wall.transform.position.y;
        //StartCoroutine("boss");
	
	}
	
	// Update is called once per frame
	void Update () {
        if (player.Health <= 0)
        {
            StopCoroutine("boss");
            //StopCoroutine(wallTrigger.gameCoroutine);
            StartCoroutine("WallGoUp");
            wallTrigger.CoroutineStarted = false;
            transform.position = spots[3].position;
            Health = MaxHealth;
        }


        if (Health <= 0 && !dead)
        {
            dead = true;
            //StopCoroutine(wallTrigger.gameCoroutine);
            Instantiate(DestroyedEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);
            Destroy(EndWall);
        }

        Animator.SetBool("vulnerable", vulnerable);
        Animator.SetBool("isJumping", isJumping);
	
	}
    public void StopLocalCoroutine()
    {
        StopCoroutine("boss");
    }

    public void StartBossCo()
    {
        StartCoroutine(boss());

    }
    public IEnumerator boss()
    {
        while (true)
        {
            //move towards right
            transform.localScale = new Vector2(-1, 1);
            HealthBar.transform.localScale = new Vector2((HealthBar.transform.localScale.x * -1), HealthBar.transform.localScale.y);
            while (transform.position.x != spots[0].position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(spots[0].position.x, spots[0].position.y), speed);

                yield return null;
            }

             transform.localScale = new Vector2(1,1);
             HealthBar.transform.localScale = new Vector2((HealthBar.transform.localScale.x * -1), HealthBar.transform.localScale.y);

            yield return new WaitForSeconds(1f);

            //first attack
            int i = 0;
            while (i < 5)
            {
                GameObject bullet = (GameObject)Instantiate(projectile, shootSpots[UnityEngine.Random.Range(0, 2)].position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(SpawnProjectileSound, transform.position);
                bullet.GetComponent<Rigidbody2D>().velocity = -Vector2.right * projectileSpeed;

                if (Animator != null)
                {
                    Animator.SetTrigger("Shoot");
                }

                i++;
                yield return new WaitForSeconds(1f);
            }

            //second attack
            

            int j = 0;
            while (j < 4)
            {
                while (transform.position != spots[2].position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, spots[2].position, speed);
                    isJumping = true;
                    yield return null;
                    
                }
                isJumping = false;
                playerPosition = player.transform.position;

                yield return new WaitForSeconds(2f);

                if (Animator != null)
                {
                    Animator.SetTrigger("Jump");
                }

                while (transform.position != spots[3].position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, spots[3].position, jumpSpeed);
                    yield return null;
                }
                cameraShake.Shake();

                yield return new WaitForSeconds(2f);
                j++;

            }
            this.tag = "Untagged";
            vulnerable = true;
            HealthBar.MaxHealthColor = new Color(255f, 0, 0);
            HealthBar.MinHealthColor = new Color(255f, 0, 0);
            yield return new WaitForSeconds(4f);
            this.tag = "Deadly";
            vulnerable = false;
            HealthBar.MaxHealthColor = new Color(0, 255f, 0);
            HealthBar.MinHealthColor = new Color(0, 255f, 0);


            

            //while (transform.position.x != playerPosition.x)
            //{
            //    transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPosition.x, playerPosition.y), speed);

            //    yield return null;
            //}

           

            //third attack
            //Transform temp;
            //if (transform.position.x > Player.transform.position.x)
            //    temp = spots[1];
            //else
            //    temp = spots[0];

            //while (transform.position.x != temp.position.x)
            //{
            //    transform.position = Vector2.MoveTowards(transform.position, new Vector2(temp.position.x, temp.position.y), speed);
            //    yield return null;
            //}

        }
    }

    IEnumerator WallGoUp()
    {
        while (Wall.transform.position.y != origWallPosition.y)
        {
            Wall.transform.position = Vector2.MoveTowards(Wall.transform.position, new Vector2(origWallPosition.x, origWallPosition.y), speed);

            yield return null;
        }

    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (vulnerable)
        {
            Health -= 10;

        }
    }
    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.collider.tag == "Player")
    //    {
    //        Console.WriteLine("asd");
    //    }
    //}
}
