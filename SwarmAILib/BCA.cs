using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAILib
{
    public class Bee
    {
        private List<(int, int)> path;

        public List<(int, int)> Path
        {
            get { return path; }
        }

        public Bee()
        {
            path = new List<(int, int)>();
        }

        public void FindPath(Maze maze)
        {
            int currentRow = 0;
            int currentCol = 0;
            Random random = new Random();

            while (!maze.IsExit(currentRow, currentCol))
            {
                path.Add((currentRow, currentCol));
                List<(int, int)> possibleMoves = GetPossibleMoves(currentRow, currentCol, maze);

                if (possibleMoves.Count == 0)
                    break;

                int chosenMoveIndex = random.Next(possibleMoves.Count);
                (currentRow, currentCol) = possibleMoves[chosenMoveIndex];
            }
        }

        private List<(int, int)> GetPossibleMoves(int row, int col, Maze maze)
        {
            List<(int, int)> possibleMoves = new List<(int, int)>();

            if (!maze.IsWall(row - 1, col)) possibleMoves.Add((row - 1, col));
            if (!maze.IsWall(row + 1, col)) possibleMoves.Add((row + 1, col));
            if (!maze.IsWall(row, col - 1)) possibleMoves.Add((row, col - 1));
            if (!maze.IsWall(row, col + 1)) possibleMoves.Add((row, col + 1));

            return possibleMoves;
        }
    }

    public class BeeColonyAlgorithm
    {
        private Maze maze;
        private int beeCount;
        private int iterations;

        public BeeColonyAlgorithm(Maze maze, int beeCount, int iterations)
        {
            this.maze = maze;
            this.beeCount = beeCount;
            this.iterations = iterations;
        }

        public List<(int, int)> FindShortestPath()
        {
            List<(int, int)> shortestPath = null;
            int shortestPathLength = int.MaxValue;

            for (int iter = 0; iter < iterations; iter++)
            {
                List<(int, int)> currentShortestPath = null;
                int currentShortestPathLength = int.MaxValue;

                for (int beeIndex = 0; beeIndex < beeCount; beeIndex++)
                {
                    Bee bee = new Bee();
                    bee.FindPath(maze);

                    int pathLength = bee.Path.Count;

                    if (pathLength < currentShortestPathLength)
                    {
                        currentShortestPathLength = pathLength;
                        currentShortestPath = bee.Path;
                    }
                }

                if (currentShortestPathLength < shortestPathLength)
                {
                    shortestPathLength = currentShortestPathLength;
                    shortestPath = currentShortestPath;
                }
            }

            return shortestPath;
        }
    }
}
