using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNormalCrystalCollision : MonoBehaviour {
    public ParticleSystem NormalSmash;
    public SoundManager SoundManager;

	void Start () {
        SoundManager = GameObject.Find("Sounds").GetComponent<SoundManager>();
        NormalSmash = GameObject.Find("NormalCrystalParticle").GetComponent<ParticleSystem>();
    }
	
    private void OnParticleCollision(GameObject other)
    {
        if (this.gameObject.tag!="FireCrystal")
        {
            
            if(other.GetComponent<ParticleSystem>().main.startColor.color == Color.yellow)
            {
                SoundManager.PlaySmashSound();
                InitGame.TotalObjectsDestroyed++;
                NormalSmash.transform.position = this.gameObject.transform.position;
                NormalSmash.Play();          
                Destroy(this.gameObject);
            }else if(other.GetComponent<ParticleSystem>().main.startColor.color == Color.red)
            {
                this.gameObject.GetComponent<CrystalHealth>().CrystalHP = 2;
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("FlameAnimator") as RuntimeAnimatorController;
                this.gameObject.tag = "FireCrystal";
            }else if(other.GetComponent<ParticleSystem>().main.startColor.color == Color.cyan)
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("IceCrystal", typeof(GameObject)), new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.Euler(0, 0, 0)) as GameObject;
                Destroy(this.gameObject);
            }

        }
      
    }
}
