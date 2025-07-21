using System.Linq;
using LaserAssetPackage.Scripts.Laser.Exceptions;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime
{
    /// <summary>
    /// Handles adding a test scene to the build, loading it and after the tests executed removing it from the build.
    /// </summary>
    [TestFixture]
    [SingleThreaded]
    public abstract class RuntimeTest
    {
        protected abstract string GetSceneName();

        [OneTimeSetUp]
        public void SetUp()
        {
            LoadScene();
        }

        private void LoadScene()
        {
            string scenePath = FindCurrentScenePath();
            if (scenePath == null)
            {
                throw new MissingLaserAssetException($"Scene {GetSceneName()} is missing or could not be loaded.");
            }

#if UNITY_EDITOR
            EditorSceneManager.LoadSceneInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
#else
            SceneManager.LoadScene(scenePath, LoadSceneMode.Single);
#endif
        }

        private string FindCurrentScenePath()
        {
            return AssetDatabase
                .FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .First(s => s.Contains(GetSceneName()));
        }
    }
}