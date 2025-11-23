using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private GameObject spawnedBullet;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject ak;
    [SerializeField] private Rigidbody bulletRB;
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam;
    

    [SerializeField] private float cooldown = 0.4f;
    [SerializeField] private float _nextFireTime;
    private string gun = "pistol";
    private float bulletSpeed = 100f;
    private float dmg;
    private float bulletLifeTime = 5f;
    public int bulletAmt = 1;
    public int spread = 2;

    private void Update()
    {
        if (pistol.activeSelf == true)
        {
            bulletAmt = 1;
            dmg = 10;
            cooldown = 0.1f;
            spread = 1;
            Debug.Log("gun1");

        }
        else if (shotgun.activeSelf == true)
        {
            gun = "shotgun";
            bulletAmt = 10;
            dmg = 5;
            cooldown = 1;
            spread = 5;
            Debug.Log("gun2");

        }
        //else if (ak.activeSelf == true)
        //{
        //    gun = "ak";
        //    bulletAmt = 1;
        //    dmg = 15;
        //    cooldown = 0.2f;
        //    Debug.Log("gun3");
        //    spread = 2;
        //}

    }

    public bool IsCoolingDown => Time.time < _nextFireTime;
    public void StartCooldowm()
    {
        _nextFireTime = Time.time + cooldown;
    }

    public void Start()
    {
        StartCooldowm();
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed && IsCoolingDown == false)
        {
            if(gun == "shotgun")
            {
                for(int i = 0; i < bulletAmt; i++)
                { 
                    spawnedBullet = Instantiate(bullet, this.transform.position, Quaternion.Euler(cam.eulerAngles.x + Random.Range(-spread, spread) + 90f, player.eulerAngles.y + Random.Range(-spread, spread), 0));
                    bulletRB = spawnedBullet.GetComponent<Rigidbody>();
                    if (bulletRB != null)
                    {
                        bulletRB.linearVelocity = spawnedBullet.transform.up * bulletSpeed;
                        Destroy(spawnedBullet, bulletLifeTime);
                    }
                }
            }
            else
            {
                spawnedBullet = Instantiate(bullet, this.transform.position, Quaternion.Euler(cam.eulerAngles.x + Random.Range(-spread, spread) + 90f, player.eulerAngles.y + Random.Range(-spread, spread), 0));
                bulletRB = spawnedBullet.GetComponent<Rigidbody>();
                if (bulletRB != null)
                {
                    bulletRB.linearVelocity = spawnedBullet.transform.up * bulletSpeed;
                    Destroy(spawnedBullet, bulletLifeTime);
                }
            }
                StartCooldowm();
        }
    }
}
