using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
