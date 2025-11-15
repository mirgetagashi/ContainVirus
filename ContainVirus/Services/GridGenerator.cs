using System;
using System.Collections.Generic;
using System.Linq;

namespace ContainVirus.Services
{
    public class GridGenerator
    {
        private readonly Random _random;

        public GridGenerator()
        {
            _random = new Random();
        }

        public (int rows, int cols, int[][] grid) GenerateGrid(string difficulty)
        {
            int gridRows, gridCols;
            double infectionRate;

            switch (difficulty.ToLower())
            {
                case "random":
                    gridRows = _random.Next(3, 18); 
                    gridCols = _random.Next(3, 18); 
                    infectionRate = _random.NextDouble() * 0.25 + 0.05; 
                    break;
                case "easy":
                    gridRows = _random.Next(3, 6); 
                    gridCols = _random.Next(3, 6); 
                    infectionRate = _random.NextDouble() * 0.05 + 0.05; 
                    break;
                case "intermediate":
                    gridRows = _random.Next(6, 11);
                    gridCols = _random.Next(6, 11); 
                    infectionRate = _random.NextDouble() * 0.10 + 0.10; 
                    break;
                case "hard":
                    gridRows = _random.Next(11, 16); 
                    gridCols = _random.Next(11, 16); 
                    infectionRate = _random.NextDouble() * 0.10 + 0.20; 
                    break;
                default:
                    gridRows = 5;
                    gridCols = 5;
                    infectionRate = 0.1;
                    break;
            }

            gridRows = Math.Min(gridRows, 20);
            gridCols = Math.Min(gridCols, 20);

            int[][] grid = new int[gridRows][];
            for (int i = 0; i < gridRows; i++)
            {
                grid[i] = new int[gridCols];
            }

            int totalCells = gridRows * gridCols;
            HashSet<(int r, int c)> infectedCells = new HashSet<(int r, int c)>();

            if (difficulty.ToLower() == "easy")
            {
                int numZones = _random.Next(1, 3); 
                int targetInfected = Math.Max(1, (int)(totalCells * infectionRate));

                if (numZones == 1)
                {
                    CreateConnectedZone(gridRows, gridCols, infectedCells, targetInfected, targetInfected);
                }
                else
                {
                    int zone1Size = targetInfected / 2;
                    int zone2Size = targetInfected - zone1Size;
                    CreateConnectedZone(gridRows, gridCols, infectedCells, Math.Max(1, zone1Size), Math.Max(1, zone1Size + 2));
                    CreateConnectedZone(gridRows, gridCols, infectedCells, Math.Max(1, zone2Size), Math.Max(1, zone2Size + 2));
                }
            }
            else
            {
                int numInfected = Math.Max(1, (int)(totalCells * infectionRate));
                while (infectedCells.Count < numInfected)
                {
                    int r = _random.Next(gridRows);
                    int c = _random.Next(gridCols);
                    infectedCells.Add((r, c));
                }
            }

            foreach (var (r, c) in infectedCells)
            {
                grid[r][c] = 1;
            }

            return (gridRows, gridCols, grid);
        }

        private void CreateConnectedZone(int rows, int cols, HashSet<(int r, int c)> infectedCells, int minSize, int maxSize)
        {
            int[][] directions = new int[][] { new[] { -1, 0 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { 0, 1 } };
            int zoneSize = _random.Next(minSize, maxSize + 1);

            int startR, startC;
            int attempts = 0;
            do
            {
                startR = _random.Next(rows);
                startC = _random.Next(cols);
                attempts++;
            } while (infectedCells.Contains((startR, startC)) && attempts < 100);

            if (attempts >= 100) return; 

            Queue<(int r, int c)> queue = new Queue<(int r, int c)>();
            HashSet<(int r, int c)> visited = new HashSet<(int r, int c)>();
            visited.Add((startR, startC));
            infectedCells.Add((startR, startC));
            int currentZoneSize = 1;

            queue.Enqueue((startR, startC));

            while (queue.Count > 0 && currentZoneSize < zoneSize)
            {
                var (r, c) = queue.Dequeue();

                var shuffledDirs = directions.OrderBy(x => _random.Next()).ToArray();

                foreach (var dir in shuffledDirs)
                {
                    if (currentZoneSize >= zoneSize) break;

                    int newR = r + dir[0];
                    int newC = c + dir[1];
                    var key = (newR, newC);

                    if (newR >= 0 && newR < rows &&
                        newC >= 0 && newC < cols &&
                        !visited.Contains(key) &&
                        !infectedCells.Contains(key))
                    {
                        visited.Add(key);
                        infectedCells.Add(key);
                        currentZoneSize++;
                        queue.Enqueue(key);
                    }
                }
            }
        }
    }
}

