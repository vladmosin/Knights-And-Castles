﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    [ExecuteInEditMode]
    public class CheckeredButtonBoard : MonoBehaviour
    {
        private GameObject parentObject;
        private Button patternButton;

        public int Width = 8;
        public int Height = 10;
        private const float SPACE_BETWEEN_BUTTONS = -2; //-2.44f; //buttonWidth/20;

        private static float ButtonWidth;
        private static float ButtonHeight;
        

        private Button[,] boardButtons;

        public void Start()
        {
            patternButton = GameObject.Find("PatternButton").GetComponent<Button>();
            parentObject = GameObject.Find("Board");

            ButtonWidth = patternButton.GetComponent<RectTransform>().rect.width;
            ButtonHeight = patternButton.GetComponent<RectTransform>().rect.height;

            Reset();
        }

        public void Reset()
        {
            DeleteButtons();
            boardButtons = new Button[Width + 1, Height + 1];
            CreateButtons();
        }

        public BoardButton GetBoardButton(IntVector2 position)
        {
            return boardButtons[position.x, position.y].gameObject.GetComponent<BoardButton>();
        }

        public void Activate()
        {
            var buttons = Object.FindObjectsOfType(typeof(Button));
            foreach (Button button in buttons.Cast<Button>().Where(button => button.gameObject.name.Contains("Clone")))
            {
                button.gameObject.SetActive(true);
            }
        }

        public void Deactivate()
        {
            var buttons = Object.FindObjectsOfType(typeof(Button));
            foreach (Button button in buttons.Cast<Button>().Where(button => button.gameObject.name.Contains("Clone")))
            {
                button.gameObject.SetActive(false);
            }
        }
        
        public void EnableBoard()
        {
            var buttons = Object.FindObjectsOfType(typeof(Button));
            foreach (Button button in buttons.Cast<Button>().Where(button => button.gameObject.name.Contains("Clone")))
            {
                button.gameObject.GetComponent<BoardButton>().Enable();
            }
        }

        public void DisableBoard()
        {
            var buttons = Object.FindObjectsOfType(typeof(Button));
            foreach (Button button in buttons.Cast<Button>().Where(button => button.gameObject.name.Contains("Clone")))
            {
                button.gameObject.GetComponent<BoardButton>().Disable();
            }
        }

        public void DeleteButtons()
        {
            Debug.Log("Deleting buttons");
            var buttons = Object.FindObjectsOfType(typeof(Button));
            foreach (Button button in buttons.Cast<Button>().Where(button => button.gameObject.name.Contains("Clone")))
            {
                Object.DestroyImmediate(button.gameObject);
            }
        }
        public static Vector3 GetOffsetFromPattern(int currentColumn, int currentRow)
        {
            return new Vector3((currentColumn - 1) * (ButtonWidth + SPACE_BETWEEN_BUTTONS),
                                                 (currentRow - 1) * (ButtonHeight + SPACE_BETWEEN_BUTTONS));
        }

        //PatternButton is placed in the bottom-left corner.
        private void CreateButtons()
        {
            Debug.Log("Creating buttons");
            for (int currentRow = 1; currentRow <= Height; currentRow++)
            {
                for (int currentColumn = 1; currentColumn <= Width; currentColumn++)
                {
                    Vector3 offset = GetOffsetFromPattern(currentColumn, currentRow);
                    Button newButton = Object.Instantiate(patternButton);
                    RectTransform rectTransform = newButton.GetComponent<RectTransform>();

                    //This line seems to be useless (it doesn't change size)
                    rectTransform.rect.size.Set(ButtonWidth, ButtonHeight);

                    rectTransform.position = patternButton.transform.localPosition + offset;
                    rectTransform.SetParent(parentObject.transform, false);

                    newButton.gameObject.SetActive(true);
                    newButton = InitButton(newButton, currentColumn, currentRow);

                    boardButtons[currentColumn, currentRow] = newButton;
                }
            }
        }

        private Button InitButton(Button newButton, int x, int y)
        {
            BoardButton boardButton = newButton.GetComponent<BoardButton>();
            boardButton.Initialize(x, y);
            return boardButton.GetComponent<Button>();
        }
    }
}