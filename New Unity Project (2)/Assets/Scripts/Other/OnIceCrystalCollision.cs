using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnIceCrystalCollision : MonoBehaviour
{
    public ParticleSystem IceSmash;
    public SoundManager SoundManager;

    void Start()
    {
        SoundManager = GameObject.Find("Sounds").GetComponent<SoundManager>();
        IceSmash = GameObject.Find("IceCrystalParticle").GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {

        if (this.gameObject.tag != "FireCrystal")// fires must not get destroyed.
        {
            if (other.GetComponent<ParticleSystem>().main.startColor.color == Color.yellow && this.gameObject.GetComponent<CrystalHealth>().CrystalHP == 0)
            {
                SoundManager.PlaySmashSound();
                InitGame.TotalObjectsDestroyed++;
                IceSmash.transform.position = this.gameObject.transform.position;
                IceSmash.Play();           
                Destroy(this.gameObject);
            }
            else if (other.GetComponent<ParticleSystem>().main.startColor.color == Color.yellow && this.gameObject.GetComponent<CrystalHealth>().CrystalHP == 1)
            {
                SoundManager.PlayCrackSound();
                this.gameObject.GetComponent<CrystalHealth>().CrackCrystal();
            }
            else if (other.GetComponent<ParticleSystem>().main.startColor.color == Color.red )
            {
                this.gameObject.GetComponent<CrystalHealth>().CrystalHP = 2;
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("FlameAnimator") as RuntimeAnimatorController;
                this.gameObject.tag = "FireCrystal";
            }
           /*else if (other.GetComponent<ParticleSystem>().main.startColor.color == Color.cyan )
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("IceCrystal", typeof(GameObject)), new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.Euler(0, 0, 0)) as GameObject;
                Destroy(this.gameObject);
            }*/

        }

    }
}
