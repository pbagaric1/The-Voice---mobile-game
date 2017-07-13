
using UnityEngine;
public class PathProjectileSpawner : MonoBehaviour
{
    public Player Player { get; private set; }
    public Transform Destination;
    public PathedProjectile Projectile;

    public GameObject SpawnEffect;
    public float Speed;
    public float FireRate;
    public AudioClip SpawnProjectileSound;
    public Animator Animator;

    private float _nextShowInSeconds;

    public void Start()
    {
        Player = FindObjectOfType<Player>();
        _nextShowInSeconds = FireRate;
    }

    public void Update()
    {
        var distanceToNextCheckpoint = transform.position.x - Player.transform.position.x;
        if (Mathf.Abs(distanceToNextCheckpoint) >= 30)
            return;

        if ((_nextShowInSeconds -= Time.deltaTime) > 0)
            return;

        _nextShowInSeconds = FireRate;
        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(Destination, Speed);

        if (SpawnEffect != null)
            Instantiate(SpawnEffect, transform.position, transform.rotation);

        if (SpawnProjectileSound != null)
            AudioSource.PlayClipAtPoint(SpawnProjectileSound, transform.position);

        if (Animator != null)
            Animator.SetTrigger("Fire");
    }

    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}

