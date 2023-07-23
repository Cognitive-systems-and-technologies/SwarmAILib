using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAILib
{
    public class Particle
    {
        private List<(int, int)> path;
        private List<(int, int)> bestPath;
        private int bestPathLength;
        private int currentPathLength;

        public List<(int, int)> Path
        {
            get { return path; }
        }

        public Particle()
        {
            path = new List<(int, int)>();
            bestPath = new List<(int, int)>();
            bestPathLength = int.MaxValue;
            currentPathLength = 0;
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

            currentPathLength = path.Count;

            if (currentPathLength < bestPathLength)
            {
                bestPathLength = currentPathLength;
                bestPath = new List<(int, int)>(path);
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

        public int BestPathLength
        {
            get { return bestPathLength; }
        }

        public List<(int, int)> BestPath
        {
            get { return bestPath; }
        }
    }

    public class ParticleSwarmOptimization
    {
        private Maze maze;
        private int particleCount;
        private int iterations;
        private List<Particle> particles;

        public ParticleSwarmOptimization(Maze maze, int particleCount, int iterations)
        {
            this.maze = maze;
            this.particleCount = particleCount;
            this.iterations = iterations;
            particles = new List<Particle>();
        }

        public List<(int, int)> FindShortestPath()
        {
            InitializeParticles();

            for (int iter = 0; iter < iterations; iter++)
            {
                foreach (Particle particle in particles)
                {
                    particle.FindPath(maze);
                }
            }

            Particle bestParticle = GetBestParticle();
            return bestParticle.BestPath;
        }

        private void InitializeParticles()
        {
            particles.Clear();
            for (int i = 0; i < particleCount; i++)
            {
                Particle particle = new Particle();
                particles.Add(particle);
            }
        }

        private Particle GetBestParticle()
        {
            Particle bestParticle = particles[0];
            foreach (Particle particle in particles)
            {
                if (particle.BestPathLength < bestParticle.BestPathLength)
                {
                    bestParticle = particle;
                }
            }

            return bestParticle;
        }
    }

}
