using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TestLightManager
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator LightManagerWithEnumeratorPasses()
    {
        //Setup test
        SceneManager.LoadScene("PlayGround");
        yield return null; //Pass one frame
        LightManager lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        
        // Check if the lights in the playground scene were found
        Assert.AreNotEqual(lightManager.GetLights().Length, 0);
    }

    [UnityTest]
    public IEnumerator GetPointingLightsWithEnumeratorPasses()
    {
        SceneManager.LoadScene("PlayGround");
        yield return null; //Pass one frame
        LightManager lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        Transform lightTransform = lightManager.gameObject.GetComponentInChildren<Light>().transform;
        Vector3 testPoint = lightTransform.position + lightTransform.forward;
        Vector3 testPointBehind = lightTransform.position - lightTransform.forward; 

        //Check if at least 1 light sees the point
        Assert.AreNotEqual(lightManager.GetPointingLights(testPoint).Length, 0);  
        //Check that no light sees the point
        Assert.AreEqual(lightManager.GetPointingLights(testPointBehind).Length, 0);  

        yield return null;
    }
}
