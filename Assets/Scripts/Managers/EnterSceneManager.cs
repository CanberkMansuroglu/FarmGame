using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class EnterSceneManager : MonoBehaviour
    {

        public void LoadGame()
        {
            SceneManager.LoadScene(1);
        }

        public void NewGame()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(1);
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        public void ReloadPP()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}