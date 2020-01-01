using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for adding and maintaining characters in the scene.
/// </summary>
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public static Dictionary<string, Vector2> stagePositions = new Dictionary<string, Vector2> {
        {"farLeft",  new Vector2(  -1f, 0f )},
        {"left",     new Vector2(   0f, 0f )},
        {"center",   new Vector2( 0.5f, 0f )},
        {"right",    new Vector2(   1f, 0f )},
        {"farRight", new Vector2(   2f, 0f )},
    };

    /// <summary>
    /// All characters must be attached to the character panel
    /// </summary>
    public RectTransform characterPanel;

    /// <summary>
    /// A list of all characters currently in the scene.
    /// </summary>
    public List<Character> characters = new List<Character>();

    /// <summary>
    /// Easy lookup for our characters.
    /// </summary>
    public Dictionary<string, int> characterDictionary = new Dictionary<string,int>();

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Try to get a character from by the name provided from the character list. If it doesn't exist, it will create an instance of it.
    /// </summary>
    public Character GetCharacter(string characterName, bool enableCreatedCharacterOnStart = true)
    {
        int index = -1;
        if(characterDictionary.TryGetValue(characterName, out index))
        {
            return characters[index];
        } else
        {
            return CreateCharacter(characterName, enableCreatedCharacterOnStart);
        }
    }

    /// <summary>
    /// Create a new instance of a given character and add it to the dictionary
    /// </summary>
    public Character CreateCharacter(string characterName, bool enableOnStart = true)
    {
        Character newCharacter = new Character(characterName, enableOnStart);

        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);

        return newCharacter;
    }
    
}
