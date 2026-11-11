 using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSequenceManager : MonoBehaviour
{
    [Header("Video Player")]
    public VideoPlayer videoPlayer;
    public VideoClip firstVideo;
    public VideoClip secondVideo;

    [Header("UI Buttons")]
    public GameObject nextButton;   // shown during video 1
    public GameObject skipButton;   // shown during video 2

    [Header("Button Timing")]
    [Tooltip("Seconds each video plays before its button appears.")]
    public float buttonDelay = 5f;

    [Header("Scene To Load")]
    [Tooltip("Name of the scene to load after video 2 ends or is skipped.")]
    public string nextSceneName;

    [Header("Options")]
    [Tooltip("If true, video 2 starts automatically when video 1 ends.")]
    public bool autoAdvanceWhenFirstEnds = false;

    private int currentIndex = -1;
    private bool sequenceFinished = false;
    private Coroutine buttonRoutine;

    void OnEnable()  { videoPlayer.loopPointReached += OnVideoEnd; }
    void OnDisable() { videoPlayer.loopPointReached -= OnVideoEnd; }

    void Start()
    {
        videoPlayer.playOnAwake = false;
        PlayVideo(0);
    }

    void PlayVideo(int index)
    {
        currentIndex = index;

        // Cancel any pending reveal from the previous video
        if (buttonRoutine != null) StopCoroutine(buttonRoutine);

        videoPlayer.Stop();
        videoPlayer.clip = (index == 0) ? firstVideo : secondVideo;
        videoPlayer.Play();

        // Hide both buttons immediately
        if (nextButton) nextButton.SetActive(false);
        if (skipButton) skipButton.SetActive(false);

        // Schedule the correct one to appear after the delay
        buttonRoutine = StartCoroutine(ShowButtonAfterDelay(index));
    }

    IEnumerator ShowButtonAfterDelay(int index)
    {
        yield return new WaitForSeconds(buttonDelay);

        // Safety check: still on the same video, sequence not finished
        if (sequenceFinished || currentIndex != index) yield break;

        if (index == 0 && nextButton) nextButton.SetActive(true);
        else if (index == 1 && skipButton) skipButton.SetActive(true);
    }

    // Hook to the "Next" button's OnClick()
    public void OnNextButtonClicked()
    {
        if (sequenceFinished || currentIndex != 0) return;
        PlayVideo(1);
    }

    // Hook to the "Skip" button's OnClick()
    public void OnSkipButtonClicked()
    {
        if (sequenceFinished || currentIndex != 1) return;
        LoadNextScene();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (currentIndex == 0)
        {
            if (autoAdvanceWhenFirstEnds) PlayVideo(1);
        }
        else
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (sequenceFinished) return;
        sequenceFinished = true;

        if (buttonRoutine != null) StopCoroutine(buttonRoutine);

        videoPlayer.Stop();
        if (nextButton) nextButton.SetActive(false);
        if (skipButton) skipButton.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("VideoSequenceManager: nextSceneName is empty!");
    }
}
