using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienPhaseManager : MonoBehaviour {
    [SerializeField] private GameObject alienWeaponBlaster;
    [SerializeField] private GameObject alienWeaponCannon;
    [SerializeField] private GameObject alienWeaponCyclic;
    [SerializeField] private GameObject alienWeaponDiskSmall;
    [SerializeField] private GameObject alienWeaponDisk;
    [SerializeField] private GameObject alienWeaponLaser;
    [SerializeField] private GameObject alienWeaponRocket;

    public GameObject player;

    private GameObject[] weapons = new GameObject[4];
    private int currentPhase;
    Enemy enemyProperties;

    private void Awake()
    {
        enemyProperties = GetComponent<Enemy>();
    }
    // Use this for initialization
    void Start () {
		//begin phase 1
        currentPhase = 1;
        setWeapons(alienWeaponCyclic, alienWeaponRocket, alienWeaponDiskSmall, alienWeaponLaser);
    }
	
	// Update is called once per frame
	void Update () {
        //check health and switch to phases based on health
        int health = (int)System.Math.Floor(enemyProperties.GetHealth().CurrentVal);
        switch (health)
        {
            case 90:
                if(currentPhase != 2)
                {
                    currentPhase = 2;
                    //change weapons/params
                    setWeapons(alienWeaponCannon, alienWeaponCyclic, alienWeaponCannon, alienWeaponCyclic);
                }
                break;
            case 80:
                if (currentPhase != 3)
                {
                    currentPhase = 3;
                    //change weapons/params
                    setWeapons(alienWeaponCyclic, alienWeaponCyclic, alienWeaponCyclic, alienWeaponCyclic);
                }
                break;
            case 70:
                if (currentPhase != 4)
                {
                    currentPhase = 4;
                    //change weapons/params
                    setWeapons(alienWeaponDisk, alienWeaponCyclic, alienWeaponDisk, alienWeaponCyclic);
                }
                break;
            case 60:
                if (currentPhase != 5)
                {
                    currentPhase = 5;
                    //change weapons/params
                    setWeapons(alienWeaponRocket, alienWeaponRocket, alienWeaponRocket, alienWeaponRocket);
                }
                break;
            case 50:
                if (currentPhase != 6)
                {
                    currentPhase = 6;
                    //change weapons/params
                    setWeapons(alienWeaponDiskSmall, alienWeaponDisk, alienWeaponDiskSmall, alienWeaponDisk);
                }
                break;
            case 40:
                if (currentPhase != 7)
                {
                    currentPhase = 7;
                    //change weapons/params
                    setWeapons(alienWeaponLaser, alienWeaponDisk, alienWeaponLaser, alienWeaponDisk);
                }
                break;
            case 30:
                if (currentPhase != 8)
                {
                    currentPhase = 8;
                    //change weapons/params
                    setWeapons(alienWeaponCyclic, alienWeaponLaser, alienWeaponLaser, alienWeaponLaser);
                }
                break;
            case 20:
                if (currentPhase != 9)
                {
                    currentPhase = 9;
                    //change weapons/params
                    setWeapons(alienWeaponDisk, alienWeaponLaser, alienWeaponDiskSmall, alienWeaponLaser);
                }
                break;
            case 10:
                if (currentPhase != 10)
                {
                    currentPhase = 10;
                    //change weapons/params
                    setWeapons(alienWeaponDisk, alienWeaponLaser, alienWeaponCyclic, alienWeaponRocket);
                }
                break;
            case 5:
                if (currentPhase != 11)
                {
                    currentPhase = 11;
                    //change weapons/params
                    setWeapons(alienWeaponCyclic, alienWeaponCyclic, alienWeaponCyclic, alienWeaponCyclic);
                    for (int i = 0; i < 4; i++)
                    {
                        weapons[i].GetComponent<AlienWeaponGun>().RateOfFire = 0.1f;
                        weapons[i].GetComponent<AlienWeaponGun>().BulletObject.GetComponent<MoveForward>().Speed = 5.0f;
                    }
                }
                break;
        }
        

        //update rotation of weapons to be towards player
        for(int i = 0; i < 4; i++)
        {
            weapons[i].transform.RotateAround(this.transform.position, Vector3.forward, 50 * Time.deltaTime);
            faceObject(weapons[i], player.transform.position);
        }
    }
    private void faceObject(GameObject gameObj, Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)gameObj.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(gameObj.name.Contains("Blaster"))
        {
            angle += 90;
        }

        var target = Quaternion.AngleAxis(angle, Vector3.forward);

        gameObj.transform.rotation = Quaternion.Slerp(gameObj.transform.rotation, target, 10 * Time.deltaTime);
    }

    private void setWeapons(GameObject w1, GameObject w2, GameObject w3, GameObject w4)
    {
        for(int i = 0; i < 4; i++)
        {
            if(weapons[i] != null)
                Destroy(weapons[i]);
        }
        float offset = 0.75f;
        Vector2 pos;
        weapons[0] = Instantiate(w1, this.transform);
        pos = weapons[0].transform.localPosition;
        pos.y += offset;
        weapons[0].transform.localPosition = pos;

        weapons[1] = Instantiate(w2, this.transform);
        pos = weapons[1].transform.localPosition;
        pos.x += offset;
        weapons[1].transform.localPosition = pos;

        weapons[2] = Instantiate(w3, this.transform);
        pos = weapons[2].transform.localPosition;
        pos.y += -offset;
        weapons[2].transform.localPosition = pos;

        weapons[3] = Instantiate(w4, this.transform);
        pos = weapons[3].transform.localPosition;
        pos.x += -offset;
        weapons[3].transform.localPosition = pos;
    }
}
