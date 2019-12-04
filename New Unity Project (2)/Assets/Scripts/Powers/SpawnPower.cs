using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// attached to camera to spawn random powers 
public class SpawnPower : MonoBehaviour {

    private int WhichPowerChance;
    private int SpawnPositionX;
    public static int CrystalHP = 0;
    public static float PowerPosition;

    void Update () {

		if (InitGame.TotalObjectsDestroyed == 15)
        {
            
            InitGame.TotalObjectsDestroyed = 0;
            WhichPowerChance = Random.Range(1, 5);

            // SpawnPositionX = Random.Range(-2.5f, 2.5f);
            SpawnPositionX = Random.Range(-2, 3);
            while (SpawnPositionX == InitGame.PreviousPosition || SpawnPositionX == InitGame.PreviousPosition2)
            {
                SpawnPositionX = Random.Range(-2, 3);
            }
            PowerPosition = SpawnPositionX; // holds
            if (WhichPowerChance == 1)
            {
                Instantiate(Resources.Load("IcePower", typeof(GameObject)), new Vector3(SpawnPositionX, 5.5f,-2), Quaternion.Euler(0, 0, 0));

            } else if (WhichPowerChance == 2)
            {
                Instantiate(Resources.Load("FirePower", typeof(GameObject)), new Vector3(SpawnPositionX, 5.5f,-2), Quaternion.Euler(0, 0, 0));

                //--------FOR DEBUG PURPOSES-------------------------//
                //SpawnPositionXX = Random.Range(-2.5f, 2.5f);
                // Instantiate(Resources.Load("WaterPower", typeof(GameObject)), new Vector3(SpawnPositionXX, 7.5f, -2), Quaternion.Euler(0, 0, 0));
                //-----------------------------------------//
            }else if (WhichPowerChance == 3)
            {
                Instantiate(Resources.Load("TimePower", typeof(GameObject)), new Vector3(SpawnPositionX, 5.5f, -2), Quaternion.Euler(0, 0, 0));

            }else if (WhichPowerChance == 4)
            {
                Instantiate(Resources.Load("StarPower", typeof(GameObject)), new Vector3(SpawnPositionX, 5.5f, -2), Quaternion.Euler(0, 0, 0));
            }
        }
	}

}
