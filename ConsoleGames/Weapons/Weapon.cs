﻿using Newtonsoft.Json;
using PowerArgs.Cli;

namespace ConsoleGames
{
    public enum WeaponStyle
    {
        Primary,
        Explosive
    }

    public abstract class Weapon : ObservableObject, IInventoryItem
    {
        [JsonIgnore]
        public Character Holder { get; set; }

        [JsonIgnore]
        public abstract WeaponStyle Style { get; }

        public int AmmoAmount
        {
            get { return Get<int>(); } set { Set(value); }
        }

        public void TryFire()
        {
            if (AmmoAmount > 0 && Holder != null)
            {
                FireInternal();
                AmmoAmount--;
            }
        }

        public abstract void FireInternal();
    }
}
