using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastHandler : MonoBehaviour {
    public ParticleSystem NumbersParticle;
    public ParticleSystem FireParticle;
    public ParticleSystem IceParticle;
    public ParticleSystem StarParticle;
    public ParticleSystem NormalSmash;
    public ParticleSystem IceSmash;
    public ParticleSystem StarSmash0;
    public ParticleSystem StarSmash1;
    public ParticleSystem StarSmash2;

    public SoundManager SoundManager;

    void Start () {
        NumbersParticle = GameObject.Find("NumbersParticleObject").GetComponent<ParticleSystem>();
        FireParticle = GameObject.Find("FireParticleObject").GetComponent<ParticleSystem>();
        IceParticle = GameObject.Find("IceParticleObject").GetComponent<ParticleSystem>();
        StarParticle = GameObject.Find("StarParticleObject").GetComponent<ParticleSystem>();

        NormalSmash = GameObject.Find("NormalCrystalParticle").GetComponent<ParticleSystem>();
        IceSmash = GameObject.Find("IceCrystalParticle").GetComponent<ParticleSystem>();
        StarSmash0 = GameObject.Find("StarParticleObject0").GetComponent<ParticleSystem>();
        StarSmash1 = GameObject.Find("StarParticleObject1").GetComponent<ParticleSystem>();
        StarSmash2 = GameObject.Find("StarParticleObject2").GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        //if (Input.GetMouseButtonDown(0))
        {
            
            Touch touch = Input.GetTouch(0);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                switch (touch.phase) {

                    case TouchPhase.Began:

                        switch (hit.collider.name)
                        {
                            case "TimePower(Clone)":
                                SoundManager.PlayPowerUpSound();
                                TimePowerHandler(hit.collider.gameObject);
                                break;

                            case "IcePower(Clone)":
                                SoundManager.PlayPowerUpSound();
                                IcePowerHandler(hit.collider.gameObject);
                                break;

                            case "StarPower(Clone)":
                                SoundManager.PlayPowerUpSound();
                                StarPowerHandler(hit.collider.gameObject);
                                break;

                            case "FirePower(Clone)":
                                SoundManager.PlayPowerUpSound();
                                FirePowerHandler(hit.collider.gameObject);
                                break;

                            case "NormalCrystal(Clone)":
                                NormalCrystalHandler(hit.collider.gameObject);
                                break;

                            case "IceCrystal(Clone)":
                                IceCrystalHandler(hit.collider.gameObject);
                                break;
                        }
                        if (hit.collider.tag != "FireCrystal" && hit.collider.name != "IceCrystal(Clone)") // Do not destroy FireCrystals
                        {
                            Destroy(hit.collider.gameObject);
                        }

                        break;

                    case TouchPhase.Ended:
                        break;
                }
            }
        }
    }


    void IceCrystalHandler(GameObject HitObject)
    {
        
        switch (HitObject.GetComponent<CrystalHealth>().CrystalHP)
        {
            case 0:
                SoundManager.PlaySmashSound();
                Destroy(HitObject);           
                if (InitGame.StarPowerEnabled == 1)
                {
                    MakeStarParticlesDefensive();
                    EmitStarParticles(HitObject);
                }else if (InitGame.StarPowerEnabled == 2)
                {
                    MakeStarParticlesOffensive();
                    EmitStarParticles(HitObject);
                }
                else
                {
                    IceSmash.transform.position = HitObject.transform.position;
                    IceSmash.Play();
                }

                InitGame.TotalObjectsDestroyed++;
                break;

            case 1:
                SoundManager.PlayCrackSound();
                HitObject.GetComponent<CrystalHealth>().CrackCrystal();
                break;

            case 2:
                InitGame.PlayerHP--;
                break;
            
        }
    }

    void NormalCrystalHandler(GameObject HitObject)
    {
        if (HitObject.tag == "FireCrystal")
        {
            InitGame.PlayerHP--;
        }
        else
        {
            SoundManager.PlaySmashSound();
            if (InitGame.StarPowerEnabled == 1)
            {
                MakeStarParticlesDefensive();
                EmitStarParticles(HitObject);
            }
            else if (InitGame.StarPowerEnabled == 2)
            {
                MakeStarParticlesOffensive();
                EmitStarParticles(HitObject);
            }
            else
            {
                NormalSmash.transform.position = HitObject.transform.position;
                NormalSmash.Play();
            }

            InitGame.TotalObjectsDestroyed++;
        }
    }
    
    void TimePowerHandler(GameObject HitObject)
    {
      
        NumbersParticle.transform.position = HitObject.transform.position;
        NumbersParticle.Play();
        InitGame.TotalTimePowers += 1;
    }

    void FirePowerHandler(GameObject HitObject)
    {
        InitGame.TotalFirePowers += 1;
        FireParticle.transform.position = HitObject.transform.position;
        FireParticle.Play();
    }

    void IcePowerHandler(GameObject HitObject)
    {
        InitGame.TotalIcePowers += 1;
        IceParticle.transform.position = HitObject.transform.position;
        IceParticle.Play();
    }

    void StarPowerHandler(GameObject HitObject)
    {
        InitGame.TotalStarPowers += 1;
        StarParticle.transform.position = HitObject.transform.position;
        StarParticle.Play();
    }

    public void MakeStarParticlesOffensive()
    {
        int randomnum;
        var main0 = StarSmash0.main;
        var main1 = StarSmash1.main;
        var main2 = StarSmash2.main;

        randomnum = Random.Range(0, 2);

        if (randomnum == 0)
        {
            main0.startColor = Color.red;
            main1.startColor = Color.red;
            main2.startColor = Color.red;
        }
        else if(randomnum == 1)
        {
            main0.startColor = Color.cyan;
            main1.startColor = Color.cyan;
            main2.startColor = Color.cyan;
        }


    }

    public void MakeStarParticlesDefensive()
    {
        var main0 = StarSmash0.main;
        var main1 = StarSmash1.main;
        var main2 = StarSmash2.main;

        main0.startColor = Color.yellow;    
        main1.startColor = Color.yellow;      
        main2.startColor = Color.yellow;
    }

    void EmitStarParticles(GameObject HitObject)
    {
        if (StarSmash0.isPlaying)
        {
            if (StarSmash1.isPlaying)
            {
                StarSmash2.transform.position = HitObject.transform.position;
                StarSmash2.Play();
            }
            else
            {
                StarSmash1.transform.position = HitObject.transform.position;
                StarSmash1.Play();
            }

        }
        else if (StarSmash1.isPlaying)
        {
            if (StarSmash0.isPlaying)
            {
                StarSmash2.transform.position = HitObject.transform.position;
                StarSmash2.Play();
            }
            else
            {
                StarSmash0.transform.position = HitObject.transform.position;
                StarSmash0.Play();
            }

        }
        else if (StarSmash2.isPlaying)
        {
            if (StarSmash1.isPlaying)
            {
                StarSmash0.transform.position = HitObject.transform.position;
                StarSmash0.Play();
            }
            else
            {
                StarSmash1.transform.position = HitObject.transform.position;
                StarSmash1.Play();
            }
        }
        else
        {
            StarSmash0.transform.position = HitObject.transform.position;
            StarSmash0.Play();
        }
    }
}
