using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public int fireHealth = 100;
    private Light light;
    private int nextDecreaseTime = 0;

    // Use this for initialization
    void Start () {
        light = GetComponentInChildren<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        if (nextDecreaseTime < Time.realtimeSinceStartup && GameManager.instance.GameRunning)
        {
            nextDecreaseTime = (int)Time.realtimeSinceStartup + 3;
            // execute block of code here
            fireHealth -= 1;
            if(fireHealth == 0)
            {
                return;
            }
            AdjustFireScaleToHealth();
        }
    }

    public void IncreaseSize(int amount)
    {
        fireHealth += amount;
        if(fireHealth > 200)
        {
            fireHealth = 200;
        }
        AdjustFireScaleToHealth();
    }

    public void DecreaseSize(int amount)
    {
        fireHealth -= amount;
        if (fireHealth < 0)
        {
            fireHealth = 0;
            return;
        }
        AdjustFireScaleToHealth();
    }

    private void AdjustFireScaleToHealth()
    {
        transform.GetChild(1).localScale = Vector3.one * (fireHealth / 100f);

        float newLightValue = 1 + 20 * fireHealth / 100f;

        if(fireHealth <= 0)
        {
            light.range = 0;
            light.intensity = 0;
        }
        else
        {
            light.range = newLightValue;
            light.intensity = newLightValue;
        }
    }
}
