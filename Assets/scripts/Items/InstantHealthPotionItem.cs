﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VGDC_RPG.Players;

namespace VGDC_RPG.Items
{
    public class InstantHealthPotionItem : Item
    {
        public override PlayerEffect UseEffect
        {
            get
            {
                return new PlayerEffect(0, 0, 10, 0);
            }
        }
        public override string Category { get { return "Potions"; } }
        public override string GUIName { get { return "Potion of Health"; } }
    }
}
