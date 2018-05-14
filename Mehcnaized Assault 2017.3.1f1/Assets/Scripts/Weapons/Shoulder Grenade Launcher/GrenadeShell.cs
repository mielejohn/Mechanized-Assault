using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeShell : MonoBehaviour {

    public GameObject Explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Hit(10);
            DestroyObject();
        }
        else if(other.tag != "Player" && other.tag != "Aiming Collider")
        {
            Debug.Log("Grenade Shell hit " + other.gameObject.name);
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        GameObject Explosion_I = (GameObject)Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public IEnumerator GrenadeTimer()
    {
        yield return new WaitForSeconds(5.0f);
        if (this.gameObject != null)
        {
            DestroyObject();
        }
    }
}
