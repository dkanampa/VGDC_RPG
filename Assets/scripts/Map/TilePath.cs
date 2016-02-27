﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VGDC_RPG.Map
{
    public class TilePath
    {
        public List<Int2> listOfTiles = new List<Int2>();

        public int costOfPath = 0;

        public Int2 lastTile;

        public TileMapScript Map;

        public TilePath(TileMapScript map)
        {
            Map = map;
        }

        public TilePath(TileMapScript map, TilePath tp) : this(map)
        {
            listOfTiles = tp.listOfTiles.ToList();
            costOfPath = tp.costOfPath;
            lastTile = tp.lastTile;
        }

        public void addTile(Int2 t)
        {
            costOfPath += Map[t.X, t.Y].TileType.MovementCost;
            listOfTiles.Add(t);
            lastTile = t;
        }
    }
}
