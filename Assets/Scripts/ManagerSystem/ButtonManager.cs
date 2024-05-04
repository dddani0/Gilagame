using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void LoadScene(int idx) => SceneManager.LoadScene(idx);
    
    public void LoadScene(int sceneNumber, int moneyAmount)
    {
        ResetMoney(moneyAmount);
        LoadScene(sceneNumber);
        //
    }

    public void ResetMoney(int defaultValue) => PlayerPrefs.SetInt("Money", defaultValue);
    public void OpenRepository() => Application.OpenURL("https://github.com/marloss/Gilagame");
}