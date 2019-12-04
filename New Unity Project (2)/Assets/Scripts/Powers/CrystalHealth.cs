using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalHealth : MonoBehaviour {
    public int CrystalHP=2;
	// Use this for initialization
	void Start () {
        if (this.gameObject.name.Contains("Ice"))
        {
            CrystalHP = 1;
        }else if (this.gameObject.name.Contains("Normal"))
        {
            CrystalHP = 0;
        }
        //CrystalHP = SpawnPower.CrystalHP;
	}
	
    public void CrackCrystal()
    {
        CrystalHP -= 1;
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("CrackedCrystalAnimator", typeof(RuntimeAnimatorController));
    }

    public void SetCrystalHP(int value)
    {
        CrystalHP = value;
    }
}
