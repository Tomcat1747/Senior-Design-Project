﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
    /// <summary>
    /// Handle an eveny on a line segment.
    /// </summary>
    /// <param name="_event"></param>
    /// <param name="segment"></param>
    public static void HandleEvent(string _event, ChapterLineManager.LINE.SEGMENT segment)
    {
        if(_event.Contains("("))
        {
            string[] actions = _event.Split(' ');
            for(int i=0; i<actions.Length; i++)
            {
                NovelController.instance.HandleAction(actions[i]);
            }
            return;
        }
        
        string[] eventData = _event.Split(' ');

        switch(eventData[0])
        {
            case "txtSpd":
                Event_TextSpeed(eventData[1], segment);
                break;
            case "/txtSpd":
            segment.architect.speed = 1f;
            segment.architect.charactersPerFrame = 1;
                break;
        }
    }

    static void Event_TextSpeed(string data, ChapterLineManager.LINE.SEGMENT segment)
    {
        string[] parts = data.Split(',');
        float delay = float.Parse(parts[0]);
        int charactersPerFrame = int.Parse(parts[1]);

        segment.architect.speed = delay;
        segment.architect.charactersPerFrame = charactersPerFrame;
    }
}
