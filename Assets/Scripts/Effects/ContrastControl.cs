using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ContrastControl : MonoBehaviour
{
    Volume vol;
    ColorAdjustments colorAdjustments;
    // Start is called before the first frame update
    void Start()
    {
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out colorAdjustments);
        colorAdjustments.contrast.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
