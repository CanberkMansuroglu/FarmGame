
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public GameObject coopPanel;
        public GameObject tradePanel;
        public GameObject ahırPanel;

        public GameObject startPanel;

        public GameObject settingPanel;


        public GameObject mainCam;
        public GameObject uiCam;

        private void Start()
        {
            if (!PlayerPrefs.HasKey("First"))
            {
                PlayerPrefs.SetInt("First", 0);
                startPanel.SetActive(true);

                Cursor.visible = true;
            }
            else
            {
                startPanel.SetActive(false);
            }

            settingPanel.SetActive(false);

            coopPanel.SetActive(false);
            tradePanel.SetActive(false);
            ahırPanel.SetActive(false);
            uiCam.SetActive(false);
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
                Time.timeScale = settingPanel.activeSelf ? 0 : 1;
                Cursor.visible = settingPanel.activeSelf;


            }
        }

        public void ClosePanel()
        {
            coopPanel.SetActive(false);
            tradePanel.SetActive(false);
            ahırPanel.SetActive(false);
            mainCam.SetActive(true);
            uiCam.SetActive(false);

            Cursor.visible = false;

        }



        public void MainMenu()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

        public void CursurVisible()
        {
            Cursor.visible = false;
        }





    }
}