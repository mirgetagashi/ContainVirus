using System;
using System.Collections.Generic;

namespace ContainVirus.Solution
{
    public class Solution
    {
        // Directions
        static readonly int[] DR = { 1, -1, 0, 0 };
        static readonly int[] DC = { 0, 0, 1, -1 };

        public int ContainVirus(int[][] grid)
        {
            int m = grid.Length, n = grid[0].Length, N = m * n;
            int answer = 0;
            // Work buffers (reused every day)
            var q = new int[N];            // queue for BFS
            int[] seen1 = new int[N];      // stamp: seen infected this day
            int[] seen0 = new int[N];      // stamp: frontier cells for a given region (regionId stamp)
            int seen1Tok = 1;              // increments each day
            int regionTokStart = 10;       // base to build unique stamps per region
            // Per-region storage
            var regionCells = new List<int>();         // contiguous chunked in 'cells' list
            var regionCellsIndex = new List<int>();    // start index for each region inside 'cells'
            var cells = new List<int>(N);              // flat store of all regions' cells (packed)
            var frontiers = new List<List<int>>();     // frontier cells per region (as lists)
            var walls = new List<int>();               // walls per region

            while (true)
            {
                // discover regions
                seen1Tok++;
                regionCells.Clear();
                regionCellsIndex.Clear();
                cells.Clear();
                frontiers.Clear();
                walls.Clear();

                for (int r = 0; r < m; r++)
                {
                    for (int c = 0; c < n; c++)
                    {
                        if (grid[r][c] != 1) continue;
                        int idx = r * n + c;
                        if (seen1[idx] == seen1Tok) continue;
                        // BFS for this region
                        int qh = 0, qt = 0;
                        q[qt++] = idx;
                        seen1[idx] = seen1Tok;
                        int start = cells.Count;
                        int wallCnt = 0;
                        var frontier = new List<int>();
                        // use a unique stamp for frontier dedup in this region
                        int regionStamp = regionTokStart + frontiers.Count + 1;

                        while (qh < qt)
                        {
                            int cur = q[qh++];
                            cells.Add(cur);
                            int cr = cur / n, cc = cur % n;
                            for (int d = 0; d < 4; d++)
                            {
                                int nr = cr + DR[d], nc = cc + DC[d];
                                if ((uint)nr >= (uint)m || (uint)nc >= (uint)n) continue;
                                int nid = nr * n + nc;
                                int val = grid[nr][nc];

                                if (val == 1)
                                {
                                    if (seen1[nid] != seen1Tok)
                                    {
                                        seen1[nid] = seen1Tok;
                                        q[qt++] = nid;
                                    }
                                }
                                else if (val == 0)
                                {
                                    wallCnt++;
                                    if (seen0[nid] != regionStamp)
                                    {
                                        seen0[nid] = regionStamp;
                                        frontier.Add(nid);
                                    }
                                }
                                // val == -1 → already contained; ignore
                            }
                        }
                        regionCellsIndex.Add(start);
                        frontiers.Add(frontier);
                        walls.Add(wallCnt);
                    }
                }

                // no regions discovered → done
                if (frontiers.Count == 0) break;

                // choose region with largest frontier
                int best = -1, bestFront = 0;
                for (int i = 0; i < frontiers.Count; i++)
                {
                    int f = frontiers[i].Count;
                    if (f > bestFront)
                    {
                        bestFront = f;
                        best = i;
                    }
                }

                if (bestFront == 0) break; // nothing more can spread

                // 1) contain the best region
                answer += walls[best];
                {
                    int start = regionCellsIndex[best];
                    int end = best + 1 < regionCellsIndex.Count ? regionCellsIndex[best + 1] : cells.Count;
                    for (int k = start; k < end; k++)
                    {
                        int id = cells[k];
                        int rr = id / n, cc = id % n;
                        grid[rr][cc] = -1;
                    }
                }

                // 2) let the other regions spread
                for (int i = 0; i < frontiers.Count; i++)
                {
                    if (i == best) continue;
                    foreach (var id in frontiers[i])
                    {
                        int rr = id / n, cc = id % n;
                        if (grid[rr][cc] == 0) grid[rr][cc] = 1;
                    }
                }

                // bump the token range so region stamps never collide inside a day
                regionTokStart += frontiers.Count + 5;
            }

            return answer;
        }
    }
}
