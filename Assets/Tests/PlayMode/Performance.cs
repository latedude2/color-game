using System.Collections;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine;

public class Performance
{
    [Test, Performance]
    public void ExampleTest()
    {
        Measure.Method(Counter).Run();
    }

    [UnityTest, Performance]
    public IEnumerator Level03()
    {
        SceneManager.LoadScene("Level03");

        yield return Measure.Frames().WarmupCount(5).MeasurementCount(100).Run();
    }

    [UnityTest, Performance]
    public IEnumerator Level03LowQuality()
    {
        SceneManager.LoadScene("Level03");
        QualitySettings.SetQualityLevel(0, true);

        yield return Measure.Frames().WarmupCount(5).MeasurementCount(100).Run();
    }

    [UnityTest, Performance]
    public IEnumerator PerformanceScene()
    {
        SceneManager.LoadScene("Performance");
        yield return null;

        GameObject.Find("RailSystem").transform.Find("RailNode2").GetComponent<RailNode>().Activate();
        GameObject.Find("RailSystem (1)").transform.Find("RailNode2").GetComponent<RailNode>().Activate();
        GameObject.Find("RailSystem (2)").transform.Find("RailNode2").GetComponent<RailNode>().Activate();

        yield return Measure.Frames().WarmupCount(5).MeasurementCount(500).Run();
    }

    private static void Counter()
    {
        var sum = 0;
        for (var i = 0; i < 10000000; i++)
        {
            sum += i;
        }
    }
}
