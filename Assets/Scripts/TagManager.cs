﻿using System;
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

        public string ShopManagerTag => "ShopManager";

        //
        public string PlayerTag => "Player";
        public string CorpseTag => "Corpse";
        public string CrosshairTag => "Crosshair";
        public string ShopItemTag => "ShopItem";
    }
}