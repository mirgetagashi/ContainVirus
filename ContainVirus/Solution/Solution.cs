using System;
using System.Collections.Generic;

namespace ContainVirus.Solution
{
    public class Solution
    {
        private static readonly int[] DR = { 1, -1, 0, 0 };
        private static readonly int[] DC = { 0, 0, 1, -1 };

        private class RegionInfo
        {
            public int Id;
            public int FrontierCount;
            public int WallCount;
        }

        public int ContainVirus(int[][] grid)
        {
            int m = grid.Length;
            int n = grid[0].Length;

            int[,] regionId = new int[m, n];
            bool[] contained = new bool[m * n + 5];

            var frontierQueue = new Queue<(int r, int c, int region)>();

            int totalWalls = 0;
            int nextRegionId = 1;

            while (true)
            {
                int roundStartRegionId = nextRegionId;
                int bestRegionId = 0;
                int bestFrontierSize = 0;
                int wallsForBestRegion = 0;

                var infoPerRegion = new Dictionary<int, RegionInfo>();

                for (int r = 0; r < m; r++)
                {
                    for (int c = 0; c < n; c++)
                    {
                        if (grid[r][c] == 0) continue;

                        if (contained[regionId[r, c]]) continue;

                        if (regionId[r, c] >= roundStartRegionId) continue;

                        int currentRegionId = nextRegionId++;

                        if (contained.Length <= currentRegionId)
                        {
                            Array.Resize(ref contained, contained.Length * 2);
                        }

                        var info = new RegionInfo { Id = currentRegionId };

                        DFS(r, c, grid, m, n,
                            regionId, contained,
                            roundStartRegionId, info,
                            frontierQueue);

                        infoPerRegion[currentRegionId] = info;

                        if (info.FrontierCount > bestFrontierSize)
                        {
                            bestFrontierSize = info.FrontierCount;
                            bestRegionId = currentRegionId;
                            wallsForBestRegion = info.WallCount;
                        }
                    }
                }

                if (bestFrontierSize == 0) break;

                totalWalls += wallsForBestRegion;
                contained[bestRegionId] = true;

                while (frontierQueue.Count > 0)
                {
                    var (r, c, region) = frontierQueue.Dequeue();

                    if (region == bestRegionId) continue;

                    if (grid[r][c] == 0)
                    {
                        grid[r][c] = 1;
                        regionId[r, c] = region;
                    }
                }
            }

            return totalWalls;
        }

        private void DFS(
            int r,
            int c,
            int[][] grid,
            int m,
            int n,
            int[,] regionId,
            bool[] contained,
            int roundStartRegionId,
            RegionInfo info,
            Queue<(int r, int c, int region)> frontierQueue)
        {
            if (r < 0 || c < 0 || r >= m || c >= n) return;

            if (grid[r][c] == 1)
            {
                if (regionId[r, c] >= roundStartRegionId || contained[regionId[r, c]]) return;

                regionId[r, c] = info.Id;

                for (int d = 0; d < 4; d++)
                {
                    DFS(r + DR[d], c + DC[d],
                        grid, m, n,
                        regionId, contained,
                        roundStartRegionId, info,
                        frontierQueue);
                }
            }
            else if (grid[r][c] == 0)
            {
                info.WallCount++;

                if (regionId[r, c] != info.Id)
                {
                    info.FrontierCount++;
                    regionId[r, c] = info.Id;
                    frontierQueue.Enqueue((r, c, info.Id));
                }
            }
        }
    }
}
