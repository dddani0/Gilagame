using ManagerSystem;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private CanvasManager _canvasManager;
    private IngameManager _ingameManager;
    private ShopKeeper _currentShopkeeper;
    private bool _isBusy;

    public void ChangeShopState() => _isBusy = _isBusy is false;
    public bool IsBusy => _isBusy;
}
