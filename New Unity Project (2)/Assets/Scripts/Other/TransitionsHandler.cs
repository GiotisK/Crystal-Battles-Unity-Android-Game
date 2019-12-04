using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionsHandler : MonoBehaviour {
    public GameObject StanceButton;
    public GameObject IceButton;
    public GameObject FireButton;
    public GameObject TimeButton;
    public GameObject StarButton;
    public GameObject MyHp;
    public GameObject EnemyHp;
    public GameObject MyHpBar;
    public GameObject EnemyHpBar;

    public Animator StanceButtonAnimator;
    public Animator IceButtonAnimator;
    public Animator FireButtonAnimator;
    public Animator TimeButtonAnimator;
    public Animator StarButtonAnimator;
    public Animator MyHpAnimator;
    public Animator EnemyHpAnimator;
    public Animator MyHpBarAnimator;
    public Animator EnemyHpBarAnimator;



    // Use this for initialization
    void Start () {

        StanceButtonAnimator = StanceButton.GetComponent<Animator>();
        IceButtonAnimator = IceButton.GetComponent<Animator>();
        FireButtonAnimator = FireButton.GetComponent<Animator>();
        TimeButtonAnimator = TimeButton.GetComponent<Animator>();
        StarButtonAnimator = StarButton.GetComponent<Animator>();
        MyHpAnimator = MyHp.GetComponent<Animator>();
        EnemyHpAnimator = EnemyHp.GetComponent<Animator>();
        MyHpBarAnimator = MyHpBar.GetComponent<Animator>();
        EnemyHpBarAnimator = EnemyHpBar.GetComponent<Animator>();

}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayTransitions()
    {
        StanceButtonAnimator.SetBool("StanceButtonTransition", true);
        IceButtonAnimator.SetBool("IceButtonTransition", true);
        FireButtonAnimator.SetBool("FireButtonTransition", true);
        TimeButtonAnimator.SetBool("TimeButtonTransition", true);
        StarButtonAnimator.SetBool("StarButtonTransition", true);
        MyHpAnimator.SetBool("MyHpTransition", true);
        EnemyHpAnimator.SetBool("EnemyHpTransition", true);
        MyHpBarAnimator.SetBool("MyHpBarTransition", true);
        EnemyHpBarAnimator.SetBool("EnemyHpBarTransition", true);

    }
    
   public void ResetTransitions()
    {
        TimeButtonAnimator.SetBool("TimeButtonTransition", false);
        StanceButtonAnimator.SetBool("StanceButtonTransition", false);
        IceButtonAnimator.SetBool("IceButtonTransition", false);
        FireButtonAnimator.SetBool("FireButtonTransition", false);     
        StarButtonAnimator.SetBool("StarButtonTransition", false);
        MyHpAnimator.SetBool("MyHpTransition", false);
        EnemyHpAnimator.SetBool("EnemyHpTransition", false);
        MyHpBarAnimator.SetBool("MyHpBarTransition", false);
        EnemyHpBarAnimator.SetBool("EnemyHpBarTransition", false);
    }
}
