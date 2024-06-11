using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TagManager : MonoBehaviour
    {
        public static TagManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public string IngameManagerTag => "IngameManager";
        public string CanvasManagerTag => "CanvasManager";
        public string WeaponDisplayTag => "DisplayWeapon";
        public string ShopManagerTag => "ShopManager";

        //
        public string PlayerTag => "Player";
        public string EnemyTag => "Enemy";
        public string CorpseTag => "Corpse";
        public string CrosshairTag => "Crosshair";
        public string ShopItemTag => "ShopItem";
        public string BulletTag => "Bullet";
        public string PlayerPositionTag => "pos";
        public string TriggerTag => "trigger";
        public string SignTrigger => "signtrigger";

        // save tags
        public string MoneySaveTag => "Money";
        public string InventoryStateSaveTag => "InventoryState";
        public string PlayerPositionSaveTag => "pos";
    }
}