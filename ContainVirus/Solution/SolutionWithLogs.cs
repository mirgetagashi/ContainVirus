using ContainVirus.Models;
using System;
using System.Collections.Generic;

namespace ContainVirus.Solution
{
    public class SolutionWithLogs
    {
        static readonly int[] DR = { 1, -1, 0, 0 };
        static readonly int[] DC = { 0, 0, 1, -1 };

        public (int totalWalls, int totalDays, List<VirusStepLog> logs) ContainVirusWithLogs(int[][] grid)
        {
            int m = grid.Length, n = grid[0].Length, N = m * n;
            int answer = 0;
            var logs = new List<VirusStepLog>();
            int day = 0;

            var q = new int[N];
            int[] seen1 = new int[N];
            int[] seen0 = new int[N];
            int seen1Tok = 1;
            int regionTokStart = 10;

            var regionCellsIndex = new List<int>();
            var cells = new List<int>(N);
            var frontiers = new List<List<int>>();
            var walls = new List<int>();

            while (true)
            {
                day++;
                seen1Tok++;
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
                    if (frontiers[i].Count > bestFront)
                    {
                        bestFront = frontiers[i].Count;
                        best = i;
                    }
                }
                if (bestFront == 0) break;

                answer += walls[best];
                var log = new VirusStepLog
                {
                    DayNumber = day,
                    ChosenRegionWalls = walls[best]
                };

                int startIdx = regionCellsIndex[best];
                int endIdx = best + 1 < regionCellsIndex.Count ? regionCellsIndex[best + 1] : cells.Count;
                for (int k = startIdx; k < endIdx; k++)
                {
                    int id = cells[k];
                    int rr = id / n, cc = id % n;
                    grid[rr][cc] = -1;
                    log.ChosenRegionCells.Add((rr, cc));
                }

                for (int i = 0; i < frontiers.Count; i++)
                {
                    if (i == best) continue;
                    foreach (var id in frontiers[i])
                    {
                        int rr = id / n, cc = id % n;
                        if (grid[rr][cc] == 0)
                        {
                            grid[rr][cc] = 1;
                            log.SpreadCells.Add((rr, cc));
                        }
                    }
                }

                int[][] snapshot = new int[m][];
                for (int i = 0; i < m; i++)
                {
                    snapshot[i] = new int[n];
                    Array.Copy(grid[i], snapshot[i], n);
                }
                log.GridSnapshot = snapshot;
                logs.Add(log);

                regionTokStart += frontiers.Count + 5;
            }

            return (answer, day, logs);
        }
    }
}
