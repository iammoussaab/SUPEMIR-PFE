using System;
using UnityEngine;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.BasicActorTest
{
    public class ScreenshotUtils : MonoBehaviour
    {
        public string pathWithFileName;
        public int scaleValue = 2;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var file = $"{pathWithFileName}_{GetCurrentDateTime()}.png";
                ScreenCapture.CaptureScreenshot(file);
                Debug.Log($"Saved screenshot to file {file}");
            }
        }

        private string GetCurrentDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
        }
    }
}