using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkPosition : MonoBehaviour {

    private float yPosition;
    
    void Update () {
        yPosition = this.gameObject.transform.position.y;

        if (yPosition <= -5.5f)
        {
            
            if (this.gameObject.tag == "Crystal")
            {
               // InitGame.PlayerHP--;
            }
            Destroy(this.gameObject);
        }
	}
}
