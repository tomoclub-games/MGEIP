using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.Characters
{
    [CreateAssetMenu(fileName = "CharacterArtDataContainer", menuName = "MGEIP Spreadsheet Container/ CharacterArtData", order = 5)]
    public class CharacterArtContainer : ScriptableObject
    {
        public List<CharacterArt> characterArts;
    }

    [Serializable]
    public class CharacterArt
    {
        public int scenarioNo;
        public int sceneNo;
        public Sprite mainCharacterSprite;
        public Sprite sideCharacterSprite;

        public PivotPosition mainCharacterPivot;
        public PivotPosition sideCharacterPivot;

        public Vector2 mainCharacterSize;
        public Vector2 sideCharacterSize;

        public Vector2 GetMainCharacterPivotVector()
        {
            return GetPivotVector(mainCharacterPivot);
        }

        // Method to get the actual pivot Vector2 for the side character
        public Vector2 GetSideCharacterPivotVector()
        {
            return GetPivotVector(sideCharacterPivot);
        }

        // Utility function to map the enum to the Vector2 pivot
        private Vector2 GetPivotVector(PivotPosition pivot)
        {
            switch (pivot)
            {
                case PivotPosition.Center:
                    return new Vector2(0.5f, 0.5f);
                case PivotPosition.Left:
                    return new Vector2(0, 0.5f);
                case PivotPosition.Right:
                    return new Vector2(1, 0.5f);
                case PivotPosition.TopLeft:
                    return new Vector2(0, 1);
                case PivotPosition.TopRight:
                    return new Vector2(1, 1);
                case PivotPosition.BottomLeft:
                    return new Vector2(0, 0);
                case PivotPosition.BottomRight:
                    return new Vector2(1, 0);
                default:
                    return new Vector2(0.5f, 0.5f); // Default to center
            }
        }
    }

    [Serializable]
    public class CharacterArtByScene
    {
        public int scenarioNo;
        public int sceneNo;
        public Sprite mainCharacterSprite;
        public Sprite sideCharacterSprite;

        public PivotPosition mainCharacterPivot;
        public PivotPosition sideCharacterPivot;

        public Vector2 mainCharacterSize;
        public Vector2 sideCharacterSize;

        public Vector2 GetMainCharacterPivotVector()
        {
            return GetPivotVector(mainCharacterPivot);
        }

        // Method to get the actual pivot Vector2 for the side character
        public Vector2 GetSideCharacterPivotVector()
        {
            return GetPivotVector(sideCharacterPivot);
        }

        // Utility function to map the enum to the Vector2 pivot
        private Vector2 GetPivotVector(PivotPosition pivot)
        {
            switch (pivot)
            {
                case PivotPosition.Center:
                    return new Vector2(0.5f, 0.5f);
                case PivotPosition.Left:
                    return new Vector2(0, 0.5f);
                case PivotPosition.Right:
                    return new Vector2(1, 0.5f);
                case PivotPosition.TopLeft:
                    return new Vector2(0, 1);
                case PivotPosition.TopRight:
                    return new Vector2(1, 1);
                case PivotPosition.BottomLeft:
                    return new Vector2(0, 0);
                case PivotPosition.BottomRight:
                    return new Vector2(1, 0);
                default:
                    return new Vector2(0.5f, 0.5f); // Default to center
            }
        }
    }

    public enum PivotPosition
    {
        Center,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}