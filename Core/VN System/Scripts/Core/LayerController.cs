using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LayerController : MonoBehaviour
{
    public static LayerController instance;
    public GameObject layerTemplate;
    public LAYER background = new LAYER();
    public LAYER cinematics = new LAYER();
    public LAYER foreground = new LAYER();

    void Awake()
    {
        instance = this;
        background.Init(layerTemplate);
        cinematics.Init(layerTemplate);
        foreground.Init(layerTemplate);
    }

    [System.Serializable]
    public class LAYER
    {
        public GameObject root;
        public GameObject layerTemplate;
        public RawImage activeImage;
        public List<RawImage> allImages = new List<RawImage>();

        public Coroutine transitionCoroutine = null;
        VideoClip currentVideo;

        public void Init(GameObject template){
            layerTemplate = template;
        }

        public void SetTexture(Texture texture, VideoClip videoClip = null, bool loopMovie = true)
        {
            if(activeImage != null && activeImage.texture != null)
            {
                VideoPlayer videoPlayer = activeImage.gameObject.GetComponent<VideoPlayer>();
                if(videoPlayer != null)
                    videoPlayer.Stop();
            }

            if(texture != null)
            {
                if(activeImage == null)
                    CreateNewActiveImage();

                activeImage.texture = texture;
                activeImage.color = VN_Util.SetAlpha(activeImage.color, 1f);

                VideoPlayer videoPlayer = activeImage.gameObject.GetComponent<VideoPlayer>();
                if(videoPlayer != null && videoClip != null)
                {
                    currentVideo = videoClip;
                    videoPlayer.clip = videoClip;
                    videoPlayer.isLooping = loopMovie;
                    videoPlayer.Play();
                }
                
            }else
            {
                if(activeImage != null)
                {
                    allImages.Remove(activeImage);
                    GameObject.DestroyImmediate(activeImage.gameObject);
                    activeImage = null;
                }
            }
        }

        public void TransitionToTexture(Texture texture, float speed = 2f, VideoClip videoClip = null, bool loopMovie = true)
        {
            if(activeImage != null && activeImage.texture == texture)
            {
                VideoClip vc = activeImage.GetComponent<VideoPlayer>().clip;
                if(vc == videoClip)
                    return;
            }

            StopTransitioning();
            transitioning = LayerController.instance.StartCoroutine(Transitioning(texture, speed, videoClip, loopMovie));
        }

        void StopTransitioning()
        {
            if(isTransitioning)
                LayerController.instance.StopCoroutine(transitioning);
        }

        public bool isTransitioning { get { return transitioning != null; } }
        Coroutine transitioning = null;
        IEnumerator Transitioning(Texture texture, float speed, VideoClip videoClip = null, bool loopMovie = true)
        {
            if(texture != null)
            {
                for (int i = 0; i < allImages.Count; i++)
                {
                    RawImage image = allImages[i];
                    if (image.texture == texture)
                    {
                        activeImage = image;
                        break;
                    }
                }

                
                if (activeImage == null || activeImage.texture != texture || currentVideo != videoClip)
                {
                    CreateNewActiveImage();
                    activeImage.texture = texture;
                    activeImage.color = VN_Util.SetAlpha(activeImage.color, 0f);

                    VideoPlayer videoPlayer = activeImage.gameObject.GetComponent<VideoPlayer>();
                    if (videoPlayer != null && videoClip != null)
                    {
                        currentVideo = videoClip;
                        videoPlayer.clip = videoClip;
                        videoPlayer.isLooping = loopMovie;
                        videoPlayer.Play();
                    }
                }

            } else
                activeImage = null;
            
            while(VN_Util.TransitionRawImages(ref activeImage, ref allImages, speed))
                yield return new WaitForEndOfFrame();
            
            StopTransitioning();
        }

        void CreateNewActiveImage()
        {
            GameObject ob = Instantiate(layerTemplate, root.transform) as GameObject;
            ob.SetActive(true);
            RawImage raw = ob.GetComponent<RawImage>();
            activeImage = raw;
            allImages.Add(raw);
        }

    }

}
