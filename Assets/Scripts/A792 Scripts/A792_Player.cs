using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class A792_Player : MonoBehaviour {

    public GameObject centerAimObject;
    Camera cam;
    public Transform rightEye, leftEye;
    [Header("Player Properties")]
    public int health;
    public int humanity;
    public int humanity_Max;
    public float ammo_RechargeRate;
    public float ammo_Current;
    public float ammo_Max;
    [Header("Projectile Types")]
    public GameObject pistolProjectile;
    [Header("HUD Variables")]
    public Text humanityMaxText;
    public Slider humanityBar;
    public Slider ammoBar;

    // weapons
    enum Weapon {pistol, shotgun, smg, rocket, laser, flamethrower, cannon, splinter, chargeBounceShot, sawblade};
    Weapon weaponRight, weaponLeft;
    // weapon values
    int pistol_AmmoConsump = 3, shotgun_AmmoConsump, smg_AmmoConsump, rocket_AmmoConsump, laser_AmmoConsump, flamethrower_AmmoConsump, cannon_AmmoConsump, splinter_AmmoConsump, chargeBounceShot_AmmoConsump, sawblade_AmmoConsump;
    int pistol_cooldown = 0, shotgun_cooldown, smg_cooldown, rocket_cooldown, laser_cooldown, flamethrower_cooldown, cannon_cooldown, splinter_cooldown, chargeBounceShot_cooldown, sawblade_cooldown;

    float leftEyeShotCoolDown, RightEyeShotCoolDown, leftEyeShotCoolDown_Curr, RightEyeShotCoolDown_Curr;

    // Use this for initialization
    void Start ()
    {
        cam = Camera.main;
        // start with pistols
        weaponRight = Weapon.pistol;
        weaponLeft = Weapon.pistol;

        //start with ammo
        ammo_Current = ammo_Max;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // aiming
        Ray centerAimRay = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit centerAimRayHit;

        if (Physics.Raycast(centerAimRay, out centerAimRayHit, 200))
        {
            centerAimObject.transform.position = centerAimRayHit.point;
        }
        else
        {
            centerAimObject.transform.position = (cam.transform.position + (cam.transform.forward * 100));
        }

        if (Input.GetMouseButtonDown(0))
        {
            // shoot left eye
            FireWeapon(true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            // shoot right eye
            FireWeapon(false);
        }

        // variable clamps
        humanity = Mathf.Clamp(humanity, 0, humanity_Max);
        ammo_Current = Mathf.Clamp(ammo_Current, 0, ammo_Max);

        // UI STUFF
        // humanity
        humanityBar.maxValue = humanity_Max;
        humanityBar.value = humanity;
        humanityMaxText.text = humanity.ToString();
        // ammo
        ammoBar.maxValue = ammo_Max;
        ammoBar.value = ammo_Current;

        // recharging ammo
        if (ammo_Current < ammo_Max)
        {
            ammo_Current += (ammo_RechargeRate * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {

    }

    void FireWeapon(bool shootFromLeftEye)
    {
        // are we shooting from the left eye?
        if (shootFromLeftEye)
        {
            // is our eye off cooldown?
            if (leftEyeShotCoolDown_Curr >= leftEyeShotCoolDown)
            {
                // which weapon are we using?
                if (weaponLeft == Weapon.pistol)
                {
                    // check to see if we have enough ammo to fire
                    if ((ammo_Current - pistol_AmmoConsump) >= 0)
                    {
                        // SHOOT THE PISTOL
                        // object to spawn
                        GameObject tempShot = Instantiate(pistolProjectile, leftEye.position, Quaternion.identity) as GameObject;
                        // direction to aim target
                        Vector3 dir = (centerAimObject.transform.position - leftEye.position).normalized;
                        // add force
                        tempShot.GetComponent<Rigidbody>().AddForce(dir * 70, ForceMode.VelocityChange);
                        // take away ammo
                        ammo_Current -= pistol_AmmoConsump;
                    }
                }
            }
        }
        else
        {
            // which weapon are we using?
            if (weaponRight == Weapon.pistol)
            {
                // check to see if we have enough ammo to fire
                if ((ammo_Current - pistol_AmmoConsump) >= 0)
                {
                    // object to spawn
                    GameObject tempShot = Instantiate(pistolProjectile, rightEye.position, Quaternion.identity) as GameObject;
                    // direction to aim target
                    Vector3 dir = (centerAimObject.transform.position - rightEye.position).normalized;
                    // add force
                    tempShot.GetComponent<Rigidbody>().AddForce(dir * 70, ForceMode.VelocityChange);
                    // take away ammo
                    ammo_Current -= pistol_AmmoConsump;
                }
            }
        }
    }
}
