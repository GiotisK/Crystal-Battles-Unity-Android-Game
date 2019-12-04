using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncParticle : MonoBehaviour {
    private ParticleSystem GhostParticleSys;
    GameObject GhostParticleSysObject;
    Material FireTrail;
    private bool flag = true;

    // Use this for initialization
    void Start () {
        FireTrail = (Material)Instantiate(Resources.Load("FireTrail", typeof(Material)), new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -2), Quaternion.Euler(0, 0, 0));
        if (this.gameObject.name.Equals("IceCrystal(Clone)"))
        {
            GhostParticleSysObject = (GameObject)Instantiate(Resources.Load("IceGhostTrail", typeof(GameObject)), new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -2), Quaternion.Euler(0, 0, 0));

        }
        else if (this.gameObject.name.Equals("NormalCrystal(Clone)"))
        {
            GhostParticleSysObject = (GameObject)Instantiate(Resources.Load("NormalGhostTrail", typeof(GameObject)), new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -2), Quaternion.Euler(0, 0, 0));

        }

       /* if (this.gameObject.name.Contains("Power"))
        {
            GhostParticleSysObject.SetActive(false);
        }*/
        
        GhostParticleSys = GhostParticleSysObject.GetComponent<ParticleSystem>();
        Vector3 vec3 = GhostParticleSys.transform.localScale;
        GhostParticleSysObject.transform.parent = gameObject.transform;
        GhostParticleSysObject.transform.localScale = vec3;

    }

    public void EnableTrail()
    {
        GhostParticleSysObject.gameObject.SetActive(true);
    }

    public void DisableTrail()
    {
        GhostParticleSysObject.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        GhostParticleSys.transform.position = new Vector3(this.gameObject.transform.position.x + 0.2f, this.gameObject.transform.position.y, this.gameObject.transform.position.z+1);

        if (this.gameObject.tag == "FireCrystal" && flag == true)
        {
            flag = false;
            GhostParticleSysObject.GetComponent<ParticleSystemRenderer>().material = FireTrail;
        }


	}
}
