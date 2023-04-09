using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
 
public class VideoScript : MonoBehaviour
{
    private VideoPlayer _video;

    private void Awake()
    {
        _video = GetComponent<VideoPlayer>();
        _video.Play();
        _video.loopPointReached += CheckOver;
 
         
    }
    
    private static void CheckOver(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);//the scene that you want to load after the video has ended.
    }
}