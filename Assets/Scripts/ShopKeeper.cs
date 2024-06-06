using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ManagerSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

internal class CatalogItem
{
    private Item _item;
    private bool _available;

    public CatalogItem(Item item, bool availability)
    {
        _item = item;
        _available = availability;
    }

    public void ChangeAvailability() => _available = _available is false;

    public Item GetItem => _item;

    public bool Available => _available;
}

public class ShopKeeper : MonoBehaviour
{
    public InputAction enterShop;
    private ShopManager _shopManager;
    private IngameManager _ingameManager;
    private CanvasManager _canvasManager;
    public Item[] Arsenal;
    List<CatalogItem> _items;
    private Player _player; //Get player once touched.
    private PlayerShooter _playerShooterComponent;
    private Collider2D _playercol = null;

    private void OnEnable()
    {
        enterShop.Enable();
    }

    private void OnDisable()
    {
        enterShop.Disable();
    }

    private void Start()
    {
        _canvasManager = GameObject.FindGameObjectWithTag(TagManager.Instance.CanvasManagerTag)
            .GetComponent<CanvasManager>();
        _shopManager = GameObject.FindGameObjectWithTag(TagManager.Instance.ShopManagerTag).GetComponent<ShopManager>();
        _ingameManager = GameObject.FindGameObjectWithTag(TagManager.Instance.IngameManagerTag)
            .GetComponent<IngameManager>();
        _player = GameObject.FindGameObjectWithTag(TagManager.Instance.PlayerTag).GetComponent<Player>();
        _playerShooterComponent = _player.gameObject.GetComponent<PlayerShooter>();
        _items = new List<CatalogItem>();
        foreach (var item in Arsenal)
        {
            _items.Add(new CatalogItem(item, IsItemAvailable(item)));
        }
    }

    private void Update()
    {
        EnterShop();
    }

    private void DisplayItems()
    {
        foreach (var i in _items.Where(i => i.Available != IsItemAvailable(i.GetItem)))
        {
            i.ChangeAvailability();
        }

        if (_canvasManager.shopElement.transform.childCount > 3) //Background element and button is ignored.
        {
            for (int i = _canvasManager.shopElement.transform.childCount - 1; i > 1; i--)
            {
                Destroy(_canvasManager.shopElement.transform.GetChild(i).gameObject);
            }
        }

        foreach (var shopItem in _items)
        {
            var canvasObjectParent = new GameObject
            {
                name = $"{shopItem.GetItem.name}Item",
                transform = { parent = _canvasManager.shopElement.transform },
                tag = TagManager.Instance.ShopItemTag
            };
            var itemFrame = new GameObject
            {
                name = "itemImage",
                transform = { parent = canvasObjectParent.transform }
            };
            var itemText = new GameObject
            {
                name = "itemText",
                transform =
                {
                    parent = canvasObjectParent.transform
                }
            };
            var itemPrice = new GameObject
            {
                name = "itemPrice",
                transform =
                {
                    parent = canvasObjectParent.transform
                }
            };
            canvasObjectParent.AddComponent<RectTransform>();
            if (shopItem.Available)
            {
                var button = canvasObjectParent.AddComponent<Button>();
                button.onClick.AddListener(delegate { BuyItem(shopItem); });
            }

            var itemFrameRect = itemFrame.AddComponent<RectTransform>();
            var imageComponent = itemFrame.AddComponent<Image>();
            imageComponent.sprite = shopItem.GetItem.sprite;
            var textFrameRect = itemText.AddComponent<RectTransform>();
            var textComponent = itemText.AddComponent<TMPro.TextMeshProUGUI>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.text = shopItem.GetItem.name;
            textFrameRect.position =
                new Vector3(textFrameRect.position.x, itemFrameRect.position.y - 80f); //offset (temp: hard-coded value)
            var itemPriceRect = itemPrice.AddComponent<RectTransform>();
            itemPriceRect.position =
                new Vector3(itemPriceRect.position.x, -textFrameRect.rect.height - 30f);
            var itemPriceText = itemPrice.AddComponent<TMPro.TextMeshProUGUI>();
            itemPriceText.text = shopItem.Available ? ($"{shopItem.GetItem.price.ToString()}$") : ("SOLD!");
            itemPriceText.color = shopItem.Available
                ? _player.Money < shopItem.GetItem.price ? Color.gray : Color.green
                : Color.red;
            itemPriceText.alignment = TextAlignmentOptions.Center;
        }

        return;

        bool IsItemAvailable(Item item) => PlayerPrefs.GetString(item.name).Split(";")[1].ToLower().Equals("true");
    }

    private void UpdateItem(Item selectedItem)
    {
        //Get items and cycle through items.
        var shopItems = GameObject.FindGameObjectsWithTag(TagManager.Instance.ShopItemTag);
        foreach (var item in shopItems)
        {
            if (!item.name.ToLower().Equals($"{selectedItem.name.ToLower()}item")) continue;
            item.GetComponent<Button>().enabled = false;
            var textChild = item.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            textChild.text = "SOLD!";
            textChild.color = Color.red;
            return;
        }
    }

    private void BuyItem(CatalogItem item)
    {
        if (item.Available is false) return;
        if (_player.Money < item.GetItem.price) return;
        _player.DecrementMoney(item.GetItem.price);
        PlayerPrefs.SetInt("Money", _player.Money);
        if (item.GetItem.type != ItemType.Consumeable)
        {
            item.ChangeAvailability();
            PlayerPrefs.SetString(item.GetItem.name, $"{item.GetItem.name};{item.Available}");
            UpdateItem(item.GetItem);
        }

        switch (item.GetItem.type)
        {
            case ItemType.Consumeable:
                _player.Heal(_player.GetMaxHealth - _player.GetHealth());
                break;
            case ItemType.Weapon:
                //add to player inventory.
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EnterShop()
    {
        if (_shopManager.IsBusy) return;
        if (_playercol is null) return;
        if (!_playercol.CompareTag(TagManager.Instance.PlayerTag) || !enterShop.WasPressedThisFrame()) return;
        _ingameManager.ChangePlayerActiveState();
        _ingameManager.EnableCursorVisibility();
        DisplayItems();
        _canvasManager.ShowShop();
        _shopManager.ChangeShopState();
    }

    private bool IsItemAvailable(Item item) => PlayerPrefs.GetString(item.name).Split(";")[1]
        .ToLower().ToString()
        .Equals("true");


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(TagManager.Instance.PlayerTag)) return;
        _playerShooterComponent.ShowButtonPrompter(enterShop);
        _playercol = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(TagManager.Instance.PlayerTag)) return;
        _playercol = null;
        _playerShooterComponent.DisableButtonPrompter();
    }
}