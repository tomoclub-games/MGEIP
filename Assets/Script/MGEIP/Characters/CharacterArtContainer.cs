using MGEIP.Scenario.Scenes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.Characters
{
    [CreateAssetMenu(fileName = "CharacterArtDataContainer", menuName = "MGEIP Spreadsheet Container/ CharacterArtData", order = 5)]
    public class CharacterArtContainer : ScriptableObject
    {
        public List<Character> characters;
    }

    [Serializable]
    public class Character
    {
        public int ScenarioNo;
        public List<CharacterArt> characterArts;

        public List<Scene> scenes;

    }

    [Serializable]
    public class CharacterArt
    {
        public int sceneNo;

        [SerializeField] private Sprite mainCharacterSprite;
        [SerializeField] private Sprite sideCharacterSprite;

        [SerializeField] private Vector2 mainCharacterPosition;
        [SerializeField] private Vector2 sideCharacterPosition;

        public Vector2 mainCharacterSize;
        public Vector2 sideCharacterSize;

        public Sprite GetMainCharacterSprite()
        {
            if (mainCharacterSprite != null)
                return mainCharacterSprite;
            return null;
        }

        public Sprite GetSideCharacterSprite()
        {
            if (sideCharacterSprite != null)
                return sideCharacterSprite;
            return null;
        }

        public Vector2 GetMainCharacterPositionVector() => new Vector2(mainCharacterPosition.x, mainCharacterPosition.y);

        public Vector2 GetSideCharacterPositionVector() => new Vector2(sideCharacterPosition.x, sideCharacterPosition.y);
    }

    
}