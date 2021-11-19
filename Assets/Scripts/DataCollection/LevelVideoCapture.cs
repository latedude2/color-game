using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using UnityEngine.Windows.WebCam;
using System.IO;

public class LevelVideoCapture : MonoBehaviour
{
    static readonly float MaxRecordingTime = 5.0f;

    VideoCapture m_VideoCapture = null;
    float m_stopRecordingTimer = float.MaxValue;

    void Start() {
        if (SystemInfo.operatingSystem.Contains("Windows")) {
            StartVideoCaptureTest();
        }
    }

    void Update() {
        if (SystemInfo.operatingSystem.Contains("Windows")) {
            if (m_VideoCapture == null || !m_VideoCapture.IsRecording) {
                return;
            }
        }
    }

    void OnDestroy() {
        if (SystemInfo.operatingSystem.Contains("Windows")) {
            print("Stop recording");
            m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
        }
    }

    void StartVideoCaptureTest()
    {
        Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        Debug.Log(cameraResolution);

        float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
        Debug.Log(cameraFramerate);

        VideoCapture.CreateAsync(false, delegate (VideoCapture videoCapture)
        {
            if (videoCapture != null)
            {
                m_VideoCapture = videoCapture;
                Debug.Log("Created VideoCapture Instance!");

                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.frameRate = cameraFramerate;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                m_VideoCapture.StartVideoModeAsync(cameraParameters,
                    VideoCapture.AudioState.ApplicationAndMicAudio,
                    OnStartedVideoCaptureMode);
            }
            else
            {
                Debug.LogError("Failed to create VideoCapture Instance!");
            }
        });
    }

    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Video Capture Mode!");
        string filename;
        if (SceneManager.GetActiveScene().name == "LastLevel")
            filename = string.Format("{0}.mp4", "Level9999.9");
        else
            filename = string.Format("{0}.mp4", SceneManager.GetActiveScene().name);
        string directory = System.IO.Path.Combine(Application.persistentDataPath, GameManager.Instance.session.ToString().Replace("/", "-").Replace(":", "-"));
        if (!Directory.Exists(directory)) {
            //if it doesn't, create it
            Directory.CreateDirectory(directory);
        }
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, directory, filename);
        m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Video Capture Mode!");
    }

    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");
        m_stopRecordingTimer = Time.time + MaxRecordingTime;
    }

    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }
}