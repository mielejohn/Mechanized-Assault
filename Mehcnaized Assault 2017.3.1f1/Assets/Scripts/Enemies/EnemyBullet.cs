using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum BulletType{Basic, Intermideate, Advanced};

public class EnemyBullet : MonoBehaviour {

    [EnumToggleButtons]
    public BulletType BT;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (BT)
            {
                case BulletType.Basic:
                    other.GetComponent<PlayerController>().Hit(2);
                    break;

                case BulletType.Intermideate:
                    other.GetComponent<PlayerController>().Hit(4);
                    break;

                case BulletType.Advanced:
                    other.GetComponent<PlayerController>().Hit(6);
                    break;
            }
        }
    }
}
