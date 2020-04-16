using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.DungeonSystem
{
    public enum TileType { PATH, WALL }
    public enum FillType { NON, FILLED }

    public delegate void DungeonIterator(DungeonTile tile);
    public class DungeonGeneration
    {
        public DungeonTile[,] grid
        {
            private set;
            get;
        }

        private int simulationStep = 30000;
        private int chanceToStartAlive = 45;
        private int limit = 4;
        private int caveSizeLimit = 10;

        private List<Cave> caves = new List<Cave>();

        int width;
        int height;
        System.Random rnd;
        public DungeonGeneration(int w, int h)
        {
            width = w;
            height = h;
            rnd = new System.Random();
        }

        void InitializeMap()
        {
            grid = new DungeonTile[width, height];
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    int rndValue = rnd.Next(0, 100);
                    TileType t = rndValue < chanceToStartAlive ? TileType.PATH : TileType.WALL;
                    grid[i, j] = new DungeonTile(t, i, j);
                }
            }
        }

        public void CreateMap()
        {
            InitializeMap();
            for (int i = 0; i < simulationStep; ++i)
            {
                MapIteration();
            }
            GetCaves();
        }

        void MapIteration()
        {
            int x = rnd.Next(0, width);
            int y = rnd.Next(0, height);
            int count = CountNearby(x, y);
            if (count > limit)
            {
                grid[x, y].SetTileType(TileType.PATH);
            }
            else if (count < limit)
            {
                grid[x, y].SetTileType(TileType.WALL);
            }
        }

        int CountNearby(int x, int y)
        {
            var count = 0;
            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    int nX = x + i;
                    int nY = y + j;
                    if (ValidPosition(nX, nY))
                    {
                        if (grid[nX, nY].type == TileType.PATH)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        bool ValidPosition(int x, int y)
        {
            return 0 <= x && x < width && 0 <= y && y < height;
        }

        void GetCaves()
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (grid[x, y].type == TileType.PATH && grid[x, y].fillType == FillType.NON)
                    {
                        Cave cave = FloodFill(x, y, FillType.FILLED);
                        if (cave.Size() >= caveSizeLimit)
                        {
                            caves.Add(cave);
                        }
                    }
                }
            }
            ConnectCaves();
        }

        void ConnectCaves()
        {
            for (int i = 1; i < caves.Count; ++i)
            {
                Vector2Int a = caves[i].GetRandomTile().GetPosition();
                Vector2Int b = caves[i - 1].GetRandomTile().GetPosition();
                CreateTunnel(b, a, caves[i]);
            }
        }

        void CreateTunnel(Vector2Int p1, Vector2 p2, Cave cave)
        {
            const int maxStep = 500;
            int steps = 0;
            int startX = p1.x;
            int startY = p1.y;
            DungeonTile currentTile = grid[startX, startY];
            while (steps < maxStep)
            {
                ++steps;
                double n = 1;
                double s = 1;
                double e = 1;
                double w = 1;
                double weigth = 1;
                if (startX < p2.x)
                {
                    e += weigth;
                }
                else if (startX > p2.x)
                {
                    w += weigth;
                }
                else if (startY < p2.y)
                {
                    s += weigth;
                }
                else if (startY > p2.y)
                {
                    n += weigth;
                }

                double total = n + s + e + w;
                n /= total;
                s /= total;
                e /= total;
                w /= total;

                int dx = 0;
                int dy = 0;

                double value = rnd.NextDouble();

                if (0 <= value && value < n)
                {
                    dx = 0;
                    dy = -1;
                }
                else if (n <= value && value <= n + s)
                {
                    dx = 0;
                    dy = 1;
                }
                else if (n + s <= value && value <= n + s + e)
                {
                    dx = 1;
                    dy = 0;
                }
                else
                {
                    dx = -1;
                    dy = 0;
                }
                if (ValidPosition(startX + dx, startY + dy))
                {
                    startX = startX + dx;
                    startY = startY + dy;
                    if (grid[startX, startY].type == TileType.WALL)
                    {
                        grid[startX, startY].SetTileType(TileType.PATH);
                    }
                    SetCellType(startX + 1, startY, TileType.PATH);
                    SetCellType(startX + 1, startY + 1, TileType.PATH);
                    if (cave.ContainTile(grid[startX, startY]))
                    {
                        break;
                    }
                }
            }
        }

        void SetCellType(int x, int y, TileType type)
        {
            if (ValidPosition(x, y))
            {
                grid[x, y].SetTileType(type);
            }
        }

        Cave FloodFill(int x, int y, FillType target)
        {
            Cave cave = new Cave();
            var current = grid[x, y].fillType;
            FloodFillUtil(x, y, current, target, cave);
            return cave;
        }

        private void FloodFillUtil(int x, int y, FillType current, FillType target, Cave cave)
        {
            if (!ValidPosition(x, y))
            {
                return;
            }
            var currentTile = grid[x, y];
            if (grid[x, y].fillType != current || grid[x, y].type == TileType.WALL)
            {
                return;
            }
            currentTile.SetTileFillType(target);
            cave.AddTile(currentTile);
            FloodFillUtil(x + 1, y, current, target, cave);
            FloodFillUtil(x - 1, y, current, target, cave);
            FloodFillUtil(x, y + 1, current, target, cave);
            FloodFillUtil(x, y - 1, current, target, cave);
        }

        public void IterateMap(DungeonIterator func)
        {
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    func(grid[i, j]);
                }
            }
        }
    }

    public class Cave
    {
        public List<DungeonTile> tiles { private set; get; }
        public Cave()
        {
            tiles = new List<DungeonTile>();
        }
        public void AddTile(DungeonTile tile)
        {
            tiles.Add(tile);
        }

        public int Size()
        {
            return tiles.Count;
        }

        public DungeonTile GetRandomTile()
        {
            System.Random rnd = new System.Random();
            int i = rnd.Next(0, tiles.Count);
            return tiles[i];
        }

        public bool ContainTile(DungeonTile tile)
        {
            return tiles.Contains(tile);
        }
    }

    public class DungeonTile
    {
        public int x { set; get; }
        public int y { set; get; }
        public TileType type { private set; get; }
        public FillType fillType { private set; get; }

        public DungeonTile(TileType t, int x, int y)
        {
            this.x = x;
            this.y = y;
            type = t;
            fillType = FillType.NON;
        }

        public void SetTileType(TileType t)
        {
            type = t;
        }

        public void SetTileFillType(FillType t)
        {
            fillType = t;
        }

        public Vector2Int GetPosition()
        {
            return new Vector2Int(x, y);
        }
    }
}
