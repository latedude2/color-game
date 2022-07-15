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
/*
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
    public IEnumerator Level10a()
    {
        //Setup test
        SceneManager.LoadScene("Level10a");
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

    [UnityTest]
    public IEnumerator Level12()
    {
        //Setup test
        SceneManager.LoadScene("Level12");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level13()
    {
        //Setup test
        SceneManager.LoadScene("Level13");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level13a()
    {
        //Setup test
        SceneManager.LoadScene("Level13a");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level14()
    {
        //Setup test
        SceneManager.LoadScene("Level14");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level14a()
    {
        //Setup test
        SceneManager.LoadScene("Level14a");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level15()
    {
        //Setup test
        SceneManager.LoadScene("Level15");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level16()
    {
        //Setup test
        SceneManager.LoadScene("Level16");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level17()
    {
        //Setup test
        SceneManager.LoadScene("Level17");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level18()
    {
        //Setup test
        SceneManager.LoadScene("Level18");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level19()
    {
        //Setup test
        SceneManager.LoadScene("Level19");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level20()
    {
        //Setup test
        SceneManager.LoadScene("Level20");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator Level21()
    {
        //Setup test
        SceneManager.LoadScene("Level21");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }

    [UnityTest]
    public IEnumerator LastLevel()
    {
        //Setup test
        SceneManager.LoadScene("LastLevel");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }
    */
    /*
    [UnityTest]
    public IEnumerator LevelSelect()
    {
        //Setup test
        SceneManager.LoadScene("LevelSelect");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }
    */

    [UnityTest]
    public IEnumerator StartScene()
    {
        //Setup test
        SceneManager.LoadScene("StartScene");
        yield return null; //Pass one frame

        //Check for missing prefabs
        MissingPrefabDetector.CheckForMissingPrefabsInScene();
    }
}
