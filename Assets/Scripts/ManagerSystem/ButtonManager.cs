using System;
using System.Collections.Generic;
using DefaultNamespace;
using ManagerSystem;
using PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public delegate void SelectItemDelegate(Item delegatedItem);

    public static SelectItemDelegate Select;

    public static void LoadScene(int idx) => SceneManager.LoadScene(idx);

    public static void ResetMoney(int defaultValue) => PlayerPrefs.SetInt("Money", defaultValue);
    public static void OpenRepository() => Application.OpenURL("https://github.com/marloss/Gilagame");

    public static void ResetSave(int moneyDefaultValue, Item[] items)
    {
        //Unity is being a fucking didly little shit and doesnt allow these types of functions to work with inspector value assigning.
        //we have to do it function to function
        // :(
        ResetMoney(moneyDefaultValue);
        PlayerPrefs.SetString("pos", "-118|11|0");
        foreach (var item in items)
        {
            PlayerPrefs.SetString(item.name, $"{item.name};true");
        }
    }

    public void ResetPlayPosition() => PlayerPrefs.SetString("pos", "-118|11|0");

    public void ResetItems(Item items)
    {
        // foreach (var item in items)
        // {
        //     PlayerPrefs.SetString(item.name, $"{item.name};true");
        // }
    }

    public static void AcceptBounty(GameObject bountyUI)
    {
        var ingameManager = GameObject.Find(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>();
        ingameManager.ChangePlayerActiveState();
        ingameManager.ChangeBountyStatus();
        ingameManager.SpawnEnemy();
        bountyUI.SetActive(false);
        ingameManager.ChangeCursorVisibility();
    }

    public void ExitShop()
    {
        GameObject.FindGameObjectWithTag(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>()
            .ChangePlayerActiveState();
        GameObject.FindGameObjectWithTag(TagManager.Instance.CanvasManagerTag).GetComponent<CanvasManager>()
            .DisableShop();
    }
}