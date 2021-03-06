﻿#region

using System;
using System.Collections.Generic;
using common.data;

#endregion

namespace wServer.realm.setpieces
{
    internal class BeachBum : ISetPiece
    {
        private static readonly byte Island = (byte) XmlDatas.IdToType["Ghost Water Beach"];
        private static readonly byte Water = (byte) XmlDatas.IdToType["GhostWater"];
        private static readonly byte Outer = (byte) XmlDatas.IdToType["Ghost Water Beach"];
        private static readonly short Tree = XmlDatas.IdToType["Palm Tree"];

        private readonly Random rand = new Random();

        public int Size
        {
            get { return 42; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            double sandRadius = 20;
            int waterRadius = 18;
            float islandRadius = 2.5f;
            int TreeCount = 0;

            var border = new List<IntPoint>();

            var t = new int[Size, Size];
            for (int y = 0; y < Size; y++) //Outer
                for (int x = 0; x < Size; x++)
                {
                    double dx = x - (Size/2.0);
                    double dy = y - (Size/2.0);
                    sandRadius = 20 + rand.Next(0, 3);
                    double r = Math.Sqrt(dx*dx + dy*dy);
                    if (r <= sandRadius)
                    {
                        t[x, y] = 1;
                    }
                }
            for (int y = 0; y < Size; y++) //Water
                for (int x = 0; x < Size; x++)
                {
                    double dx = x - (Size/2.0);
                    double dy = y - (Size/2.0);
                    double r = Math.Sqrt(dx*dx + dy*dy);
                    if (r <= waterRadius)
                    {
                        t[x, y] = 2;
                    }
                }

            for (int y = 0; y < Size; y++) //Island
                for (int x = 0; x < Size; x++)
                {
                    double dx = x - (Size/2.0);
                    double dy = y - (Size/2.0);
                    double r = Math.Sqrt(dx*dx + dy*dy);
                    if (r <= islandRadius)
                    {
                        int Tree = rand.Next(1, 4);

                        t[x, y] = 3;
                        if (Tree == 1 && x != 21 && y != 21 && TreeCount < 3)
                        {
                            t[x, y] = 4;
                            TreeCount++;
                        }
                    }
                }
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                {
                    if (t[x, y] == 1)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Outer;
                        tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 2)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Water;
                        tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Island;
                        tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 4)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Island;
                        tile.ObjType = Tree;
                        tile.Name = "size:" + (rand.Next()%2 == 0 ? 120 : 140);
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }
            Entity Bum = Entity.Resolve(0x0e55);
            Bum.Move(pos.X + 21.5f, pos.Y + 21.5f);
            world.EnterWorld(Bum);
        }
    }
}