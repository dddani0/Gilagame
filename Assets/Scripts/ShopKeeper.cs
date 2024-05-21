using System;
using System.Collections.Generic;
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

    public CatalogItem(Item item)
    {
        _item = item;
        _available = true;
    }

    public void ChangeAvailability() => _available = !_available;
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
        _items = new List<CatalogItem>();
        foreach (var item in Arsenal)
        {
            _items.Add(new CatalogItem(item));
        }
    }

    private void DisplayItems()
    {
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
            if (IsItemAvailable(shopItem) is false) shopItem.ChangeAvailability();
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

        bool IsItemAvailable(CatalogItem shopItem) => PlayerPrefs.GetString(shopItem.GetItem.name).Split(";")[1]
            .ToLower().ToString()
            .Equals("true");
    }

    private void UpdateItem(Item selectedItem)
    {
        //Get items and cycle through items.
        var shopItems = GameObject.FindGameObjectsWithTag("ShopItem");
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

    void BuyItem(CatalogItem item)
    {
        if (item.Available is false) return;
        item.ChangeAvailability();
        _player.DecrementMoney(item.GetItem.price);
        PlayerPrefs.SetInt("Money", _player.Money);
        PlayerPrefs.SetString(item.GetItem.name, $"{item.GetItem.name};{item.Available}");
        UpdateItem(item.GetItem);
    }

    private void updateCatalog()
    {
        
    }
    
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(TagManager.Instance.PlayerTag) && enterShop.inProgress is false) return;
        _player = other.GetComponent<Player>();
        _ingameManager.ChangePlayerActiveState();
        DisplayItems();
        _canvasManager.ShowShop();
        _shopManager.ChangeShopState();
    }
}