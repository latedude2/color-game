using System.Collections;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class LevelTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Level01()
    {
        //Setup test
        SceneManager.LoadScene("Level01");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level02()
    {
        //Setup test
        SceneManager.LoadScene("Level02");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level03()
    {
        //Setup test
        SceneManager.LoadScene("Level03");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level04()
    {
        //Setup test
        SceneManager.LoadScene("Level04");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level05()
    {
        //Setup test
        SceneManager.LoadScene("Level05");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level05a()
    {
        //Setup test
        SceneManager.LoadScene("Level05a");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level06()
    {
        //Setup test
        SceneManager.LoadScene("Level06");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level07()
    {
        //Setup test
        SceneManager.LoadScene("Level07");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level08()
    {
        //Setup test
        SceneManager.LoadScene("Level08");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level08a()
    {
        //Setup test
        SceneManager.LoadScene("Level08a");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level09()
    {
        //Setup test
        SceneManager.LoadScene("Level09");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level10()
    {
        //Setup test
        SceneManager.LoadScene("Level10");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level11()
    {
        //Setup test
        SceneManager.LoadScene("Level11");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level11a()
    {
        //Setup test
        SceneManager.LoadScene("Level11a");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level11b()
    {
        //Setup test
        SceneManager.LoadScene("Level11b");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }
}
