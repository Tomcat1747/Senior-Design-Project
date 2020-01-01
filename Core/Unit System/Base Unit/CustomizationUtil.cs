using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitDataLibrary;

namespace UnitRendererLibrary {
    public enum RenderType {
        Default, // Marks default rendering for non-customized characters.
        Custom,  // Marks renderer for full unit customization.
    }

    public enum Gender {
        Male,
        Female,
    }

    public enum Personality {
        Normal,
        LaidBack,
        Intense,
        Nervous,
        ByTheBook,
        UpBeat,
    }

    public enum UniformVersion {
        Standard,
        Winter,
    }

    [System.Serializable]
    public class UnitSpriteData {
        public Sprite[] faces;
        public Sprite[] body;
    }

    [System.Serializable]
    public class CustomSpriteData : UnitSpriteData{
        public RenderType renderType;
        
        public Gender gender;
        [Range(0, 5)] public int race;
        [Range(0, 5)] public int face;
        [Range(0, 20)] public int hair;
        [Range(0, 11)] public int facialHair;
        public Color hairColor;
        public Color eyeColor;
        public Color skinColor;
        public Personality personality;
        public ClassType classType;
        public UniformVersion uniform;
    }

}