﻿using PowerArgs.Cli;
using PowerArgs.Cli.Physics;
using System;

namespace ConsoleGames
{
    public class TimedMineDropper : Weapon
    {
        public Event Exploded { get; private set; } = new Event();

        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(3.5);

        public override WeaponStyle Style => WeaponStyle.Explosive;

        public override void FireInternal()
        {
            var mine = new TimedMine(Delay);
            mine.MoveTo(Holder.Left, Holder.Top);
            SpaceTime.CurrentSpaceTime.Add(mine);
            mine.Exploded.SubscribeOnce(this.Exploded.Fire);
        }
    }
}
