using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class MenuManager : MonoBehaviour
    {
        public void LoadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);
        public void LoadMenu() => LoadScene(0);
        public void LoadGame() => LoadScene(1);
        

        public void OpenRepository() => Application.OpenURL("https://github.com/marloss/Gilagame");
    }
}