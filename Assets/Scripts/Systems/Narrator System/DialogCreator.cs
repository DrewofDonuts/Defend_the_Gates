using UnityEngine;

namespace Etheral
{
    public class DialogCreator : MonoBehaviour
    {
       public  string text = ""; // Holds the text content
         Vector2 scrollPosition; // Tracks the scroll position

        void OnGUI()
        {
            // Begin a scroll view to handle large text
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            // Create a text area that fills the window
            text = GUILayout.TextArea(text, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            // End the scroll view
            GUILayout.EndScrollView();
        }
    }
}