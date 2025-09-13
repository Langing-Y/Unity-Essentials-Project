using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVFX : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("testvfx")]
    public GameObject effect;
    public KeyCode testKey = KeyCode.T;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(testKey))
        {
            TestEffect();
        }
    }

    public void TestEffect()
    {
        if (effect != null)
        {
            Instantiate(effect,transform.position,effect.transform.rotation);
            Debug.Log("effect out");
        }
        else
        {
            Debug.LogWarning("effect dis");
        }
    }
}
