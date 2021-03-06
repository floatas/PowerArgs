﻿using PowerArgs.Cli;
using PowerArgs.Cli.Physics;

namespace PowerArgs.Games
{
    public interface IDestructible
    {
        Event Damaged { get; }
        Event Destroyed { get; }
        float HealthPoints { get; set; }
    }

    public static class IDestructibleEx
    {
        public static void TakeDamage(this IDestructible destructible, float damage)
        {
            if(destructible is SpacialElement && (destructible as SpacialElement).HasSimpleTag("indestructible"))
            {
                return;
            }
            if (destructible.HealthPoints > 0 && damage > 0)
            {
                destructible.HealthPoints -= damage;
                if (destructible.HealthPoints <= 0)
                {
                    destructible.Damaged.Fire();
                    destructible.Destroyed.Fire();
                    if(destructible is SpacialElement)
                    {
                        (destructible as SpacialElement).Lifetime.Dispose();
                    }
                }
                else
                {
                    destructible.Damaged.Fire();
                }
            }
        }
    }
}
