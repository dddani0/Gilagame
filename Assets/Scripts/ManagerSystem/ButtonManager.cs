using System;
using System.Collections.Generic;
using DefaultNamespace;
using ManagerSystem;
using PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class ButtonManager : MonoBehaviour
{
    public Item[] items; //this is the easiest way to reset item availability (not the best solution)
    public Gun defaultGun; //ugh

    public delegate void SelectItemDelegate(Item delegatedItem);

    public static SelectItemDelegate Select;

    public static void LoadScene(int idx) => SceneManager.LoadScene(idx);

    public static void ResetMoney(int defaultValue) =>
        PlayerPrefs.SetInt(TagManager.Instance.MoneySaveTag, defaultValue);

    public static void OpenRepository() => Application.OpenURL("https://github.com/marloss/Gilagame");

    public void ResetSave(int moneyDefaultValue)
    {
        ResetMoney(moneyDefaultValue);
        ResetPlayerPosition();
        PlayerPrefs.SetString(TagManager.Instance.InventoryStateSaveTag,
            $"{defaultGun.name}##{defaultGun.ammunition}##{defaultGun.damage}##{defaultGun.fireRate}");
        foreach (var item in items)
        {
            PlayerPrefs.SetString(item.name, $"{item.name};true");
        }
    }

    public void ResetPlayerPosition() => PlayerPrefs.SetString(TagManager.Instance.PlayerPositionSaveTag, "-118|11|0");

    public static void AcceptBounty(GameObject bountyUI)
    {
        var ingameManager = GameObject.Find(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>();
        ingameManager.ChangePlayerActiveState();
        ingameManager.SpawnEnemy();
        ingameManager.ChangeBountyStatus();
        bountyUI.SetActive(false);
        ingameManager.DisableCursorVisibility();
    }

    public void ExitShop()
    {
        GameObject.FindGameObjectWithTag(TagManager.Instance.ShopManagerTag).GetComponent<ShopManager>()
            .ChangeShopState();
        var ingameManager = GameObject.FindGameObjectWithTag(TagManager.Instance.IngameManagerTag)
            .GetComponent<IngameManager>();
        ingameManager.ChangePlayerActiveState();
        ingameManager.DisableCursorVisibility();
        ingameManager.ChangePlayerActiveState();
        GameObject.FindGameObjectWithTag(TagManager.Instance.CanvasManagerTag).GetComponent<CanvasManager>()
            .DisableShop();
    }

    public void ExitInfoPanel(GameObject infoPanel)
    {
        infoPanel.SetActive(false);
        var ingameManager = GameObject.FindGameObjectWithTag(TagManager.Instance.IngameManagerTag)
            .GetComponent<IngameManager>();
        ingameManager.ChangePlayerActiveState();
        ingameManager.DisableCursorVisibility();
    }
}