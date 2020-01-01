using UnityEngine;
using System.Collections.Generic;
using System;

public class UI_Enabler : MonoBehaviour
{
    public List<Behaviour> components = new List<Behaviour>();

    // Start is called before the first frame update
    void Start()
    {
        NovelController.instance.chapterStarted += onChapterStarted;
        NovelController.instance.chapterFinished += onChapterFinished;
    }

    void onChapterStarted(object sender, EventArgs e){
        isEnabling(true);
    }

    void onChapterFinished(object sender, EventArgs e){
        isEnabling(false);
    }

    void isEnabling(bool isEnabled){
        foreach(Behaviour x in components){
            x.enabled = isEnabled;
        }
    }
}
