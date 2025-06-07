using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class UtilityControls : MonoBehaviour
    {
        public InputUtilityObject utilityObject;

        void Awake()
        {
            utilityObject.screenCaptureEvent += CaptureScreen;
        }

        public void CaptureScreen()
        {
            
                string directoryPath = "Assets/Screenshots";
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }

                string filePath = directoryPath + "/Screenshot." + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
                ScreenCapture.CaptureScreenshot(filePath);
                Debug.Log("Screenshot Captured at: " + filePath);
            
            
            //
            // ScreenCapture.CaptureScreenshot("Screenshot." + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");
            // Debug.Log("Screenshot Captured");
        }
    }
}