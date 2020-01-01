using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class SpeechPanelController : MonoBehaviour
{
    public List<GameObject> elements = new List<GameObject>();

    void Start()
    {
        NovelController.instance.chapterStarted += onChapterStarted;
        NovelController.instance.chapterFinished += onChapterFinished;
    }

    private void onChapterStarted(object sender, EventArgs e)
    {
        foreach (GameObject child in elements)
        {
            child.SetActive(true); // TODO: make this transition in.
        }
    }

    private void onChapterFinished(object sender, EventArgs e)
    {
        foreach (GameObject child in elements)
        {
            if (child.name == "Speech Box")
                child.GetComponentInChildren<TextMeshProUGUI>().text = "";
            child.SetActive(false); // TODO: make this transition in.
        }
    }

}
