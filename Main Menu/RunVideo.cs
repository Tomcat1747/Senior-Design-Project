using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RunVideo : MonoBehaviour
{
    // Create a Public Raw Image Class
    public RawImage image;
    // Create a Public Video Player Class
    public VideoPlayer video;
    // Create a Public Audio Source
    public AudioSource sound;
    // Is Called Before the First Frame Update
    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        // Prepare the Video Player
        video.Prepare();
        // Set Wait for Seconds to Wait for One Second
        WaitForSeconds waiting = new WaitForSeconds(1);
        // Repeat as Long as the Video is Not Prepared
        while(!video.isPrepared)
        {
            yield return waiting;
            break;
        }
        // Set te Image Texture to be the Same as the Video Texture
        image.texture = video.texture;
        // Play the Video
        video.Play();
        // Play the Sound
        sound.Play();
    }
}
