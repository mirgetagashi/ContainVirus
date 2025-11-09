using System;
using System.Collections.Generic;

namespace ContainVirus.Solution
{
    public class Solution
    {
        static readonly int[] DR = { 1, -1, 0, 0 };
        static readonly int[] DC = { 0, 0, 1, -1 };

        public int ContainVirus(int[][] grid)
        {
            int m = grid.Length, n = grid[0].Length, N = m * n;
            int answer = 0;
            var q = new int[N];          
            int[] seen1 = new int[N];      
            int[] seen0 = new int[N];      
            int seen1Tok = 1;            
            int regionTokStart = 10;      

            var regionCells = new List<int>();         
            var regionCellsIndex = new List<int>();    
            var cells = new List<int>(N);             
            var frontiers = new List<List<int>>();    
            var walls = new List<int>();               

            while (true)
            {
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
                        int qh = 0, qt = 0;
                        q[qt++] = idx;
                        seen1[idx] = seen1Tok;
                        int start = cells.Count;
                        int wallCnt = 0;
                        var frontier = new List<int>();

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

                            }
                        }
                        regionCellsIndex.Add(start);
                        frontiers.Add(frontier);
                        walls.Add(wallCnt);
                    }
                }

                if (frontiers.Count == 0) break;

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

                if (bestFront == 0) break; 


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


                for (int i = 0; i < frontiers.Count; i++)
                {
                    if (i == best) continue;
                    foreach (var id in frontiers[i])
                    {
                        int rr = id / n, cc = id % n;
                        if (grid[rr][cc] == 0) grid[rr][cc] = 1;
                    }
                }

                regionTokStart += frontiers.Count + 5;
            }

            return answer;
        }
    }
}
