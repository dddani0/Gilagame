using ManagerSystem;
using PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void LoadScene(int idx) => SceneManager.LoadScene(idx);

    public void ResetMoney(int defaultValue) => PlayerPrefs.SetInt("Money", defaultValue);
    public void OpenRepository() => Application.OpenURL("https://github.com/marloss/Gilagame");

    public void AcceptBounty(GameObject bountyUI)
    {
        GameObject.Find("Player").GetComponent<PlayerMovement>().ChangeActiveState(); //Játékos mozgásának beállítása
        var ingameManager = GameObject.Find("IngameManager").GetComponent<IngameManager>();
        ingameManager.ChangeBountyStatus(); //bounty mode engedélyezése
        ingameManager.SpawnEnemy(); //Ellenség lespawnolása
        bountyUI.SetActive(false); //eltüntetni a UI-t.
        ingameManager.ChangeCursorVisibility();
    }
}