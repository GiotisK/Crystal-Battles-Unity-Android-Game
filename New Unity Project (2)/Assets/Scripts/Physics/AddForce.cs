using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour {
    
    private Rigidbody2D rb;
    private Collider2D col;
    private float DefaultY;
    private float DefaultX;
    public TrailRenderer Trail;
    public AnimationCurve CustomCurve;
    public Gradient grad;
    public InitGame InitGame;
    public SyncParticle SyncParticle;
    public float timeLeft = 3.0f;
    void Start()
    {
        InitGame = GameObject.Find("Main Camera").GetComponent<InitGame>();
        SyncParticle = this.gameObject.GetComponent<SyncParticle>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        DefaultY = col.offset.y;
        DefaultX = col.offset.x;
        
    }
    private void Update()
    {
        Physics2D.SyncTransforms();

    }

    void FixedUpdate()
    {
         if (InitGame.IcePowerEnabled == 1 && gameObject.tag == "FrozenCrystal")
         {
            InitGame.TimePowerEnabled = 0;
            rb.velocity = new Vector2(0f, 0f);
            col.offset = new Vector2(0,0);
        }
        else if (InitGame.TimePowerEnabled == 1 && (gameObject.tag == "Crystal" || gameObject.tag == "FireCrystal" || gameObject.tag == "FrozenCrystal"))
        {
            if (!this.gameObject.name.Contains("Power"))
            {
                SyncParticle.EnableTrail();
            }
                
            //PlayTrailRenderer();
            rb.velocity = new Vector2(0f, -4.5f);
            col.offset = new Vector2(DefaultX, DefaultY);
            
        }
        else if (InitGame.TimePowerEnabled == 2 )
        {
            if (!this.gameObject.name.Contains("Power"))
            {
                SyncParticle.DisableTrail();
            }
                
            //CancelTrailRenderer();
            rb.velocity = new Vector2(0f, -3.0f);
            col.offset = new Vector2(DefaultX, 0.7f);
           
        }
        else if (InitGame.TimePowerEnabled == 0 )
        {
            if (!this.gameObject.name.Contains("Power"))
            {
                SyncParticle.DisableTrail();
            }

                
            //CancelTrailRenderer();
            rb.velocity = new Vector2(0f, -3.5f);
            col.offset = new Vector2(DefaultX, 1);

        }
        else if (InitGame.TimePowerEnabled == 1 && (gameObject.tag == "Crystal" || gameObject.tag == "FireCrystal"))
        {                
            col.offset = new Vector2(DefaultX, DefaultY);
        }

    }

    private void PlayTrailRenderer()
    {
        if (!this.gameObject.name.Contains("Power"))
        {
            Trail = this.gameObject.GetComponent<TrailRenderer>();
            if (this.gameObject.tag == "FireCrystal")
            {
                Trail.widthCurve = CustomCurve;

                Trail.colorGradient = grad;
            }
            Trail.emitting = true;
        }

    }

    private void CancelTrailRenderer()
    {
        if (!this.gameObject.name.Contains("Power"))
        {
            Trail = this.gameObject.GetComponent<TrailRenderer>();
            Trail.emitting = false;
        }
    }

}
