﻿using PowerArgs.Cli;
using PowerArgs;
using System.Linq;
using System;

namespace PowerArgs.Games
{
    public interface IInventoryItem { }

    public class Inventory : ObservableObject
    {
        private Lifetime itemsLifetime;
        public ObservableCollection<IInventoryItem> Items { get => Get<ObservableCollection<IInventoryItem>>(); private set => Set(value); }  

        public Character Owner { get => Get<Character>(); set => Set(value); }

        private Weapon _primaryWeapon;

        public Weapon PrimaryWeapon
        {
            get
            {
                return _primaryWeapon;
            }
            set
            {
                if (Items.Contains(value) == false)
                {
                    Items.Add(value);
                }

                _primaryWeapon = value;
                FirePropertyChanged(nameof(PrimaryWeapon));
            }
        }

        private Weapon _explosiveWeapon;

        public Weapon ExplosiveWeapon
        {
            get
            {
                return _explosiveWeapon;
            }
            set
            {
                if (Items.Contains(value) == false)
                {
                    Items.Add(value);
                }

                _explosiveWeapon = value;
                FirePropertyChanged(nameof(ExplosiveWeapon));
            }
        }

        public Inventory()
        {
            this.SubscribeForLifetime(nameof(Items), () =>
            {
                itemsLifetime?.Dispose();
                itemsLifetime = new Lifetime();
                Items.ForEach(item => ProcessItem(item));
                Items.Added.SubscribeForLifetime((item) => ProcessItem(item), itemsLifetime);

            }, this);

            this.SubscribeForLifetime(nameof(Owner), () =>
            {
                foreach(var item in Items.Where(i => i is Weapon).Select(i => i as Weapon))
                {
                    item.Holder = Owner;
                }
            }, this);

            Items = new ObservableCollection<IInventoryItem>();
        }

        public void TryEquip(Type weaponType)
        {
            var match = Items.Where(i => i is Weapon && i.GetType() == weaponType).Select(i => i as Weapon).FirstOrDefault();
            if(match == null)
            {
                return;
            }
            else if(match.Style == WeaponStyle.Primary && match.AmmoAmount > 0)
            {
                Sound.Play("switchweapon");
                PrimaryWeapon = match;
            }
            else if(match.Style == WeaponStyle.Explosive && match.AmmoAmount > 0)
            {
                Sound.Play("switchweapon");
                ExplosiveWeapon = match;
            }
        }

        private void ProcessItem(IInventoryItem item)
        {
            if (item is Weapon)
            {
                var weapon = item as Weapon;
                weapon.Holder = this.Owner;
                if (weapon.Style == WeaponStyle.Primary)
                {
                    if (PrimaryWeapon == null || PrimaryWeapon.AmmoAmount == 0)
                    {
                        PrimaryWeapon = weapon;
                    }
                }
                else
                {
                    if (ExplosiveWeapon == null || ExplosiveWeapon.AmmoAmount == 0)
                    {
                        ExplosiveWeapon = weapon;
                    }
                }
            }
        }
    }
}
