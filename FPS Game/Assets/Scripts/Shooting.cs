using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private GameObject spawnedBullet;
    public GameObject shotgun;
    public GameObject pistol;
    public GameObject ak;

    [SerializeField] private Rigidbody bulletRB;
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam;
    FirstPersonController fpc;
    private Animator sgAnimator;
    private Animator pistolAnimator;
    private Animator akAnimator;

    private bool autoFire = false;
    private bool isShooting = false;

    [SerializeField] private float cooldown = 0.4f;
    [SerializeField] private float _nextFireTime;

    public string gun;
    private float bulletSpeed = 100f;
    private float dmg;
    private float bulletLifeTime = 5f;
    public int bulletAmt = 1;
    public int spread = 2;
    public int recoil;

    private void Start()
    {
        StartCooldowm();
        sgAnimator = shotgun.GetComponent<Animator>();
        pistolAnimator = pistol.GetComponent<Animator>();
        akAnimator = ak.GetComponent<Animator>();
    }

    private void Update()
    {
        if (pistol.activeSelf)
        {
            recoil = 2;
            gun = "pistol";
            bulletAmt = 1;
            dmg = 10;
            cooldown = 0.2f;
            spread = 1;
            autoFire = false;
        }
        else if (shotgun.activeSelf)
        {
            gun = "shotgun";
            recoil = 20;
            bulletAmt = 10;
            dmg = 5;
            cooldown = 1.1f;
            spread = 5;
            autoFire = false;
        }
        else if (ak.activeSelf)
        {
            recoil = 5;
            gun = "ak";
            bulletAmt = 1;
            dmg = 15;
            cooldown = 0.2f;
            spread = 1;
            autoFire = true;
        }

        if (autoFire && gun == "ak" && isShooting && !IsCoolingDown)
        {
            akAnimator.SetTrigger("Shoot");
            FireBullet();
            StartCooldowm();
        }
    }

    public bool IsCoolingDown => Time.time < _nextFireTime;

    public void StartCooldowm()
    {
        _nextFireTime = Time.time + cooldown;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (gun == "ak")
        {
            if (context.started)
                isShooting = true;
        
            else if (context.canceled)
                isShooting = false;

            return;
        }

        if (context.performed && !IsCoolingDown)
        {
            if (gun == "shotgun")
            {
                sgAnimator.SetTrigger("Shoot");
                for (int i = 0; i < bulletAmt; i++)
                    FireBullet();
                
            }
            else if (gun == "pistol")
            {
                pistolAnimator.SetTrigger("Shoot");
                FireBullet();
            }

            StartCooldowm();
        }
    }

    private void FireBullet()
    {
        spawnedBullet = Instantiate(bullet, transform.position, Quaternion.Euler(cam.eulerAngles.x + Random.Range(-spread, spread) + 90f, player.eulerAngles.y + Random.Range(-spread, spread), 0));

        bulletRB = spawnedBullet.GetComponent<Rigidbody>();
        if (bulletRB != null)
        {
            bulletRB.linearVelocity = spawnedBullet.transform.up * bulletSpeed;
            Destroy(spawnedBullet, bulletLifeTime);
        }
        
    }
}
