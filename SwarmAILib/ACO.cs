using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAILib
{
    public class Ant
    {
        private bool[,] visited;
        private List<Coordinates> path;
        private Coordinates currentPos;

        public Ant(Maze m)
        {
            Coordinates currentPos;
            visited = new bool[m.GetRows(), m.GetColumns()];
            currentPos.x = 0;
            currentPos.y = 0;
            path = new List<Coordinates>();
            path.Add(currentPos);
        }

        public void MoveTo(int row, int col)
        {
            currentPos.x = col;
            currentPos.y = row;
            path.Add(currentPos);
        }


        public void Reset()
        {
            path = new List<Coordinates>();
            this.MoveTo(0, 0);

        }
        public int GetCurrentRow()
        {
            return currentPos.y;
        }

        public int GetCurrentCol()
        {
            return currentPos.x;
        }

        public void SetVisited(int row, int col)
        {
            visited[row, col] = true;
        }

        public bool IsVisited(int row, int col)
        {
            return visited[row, col];
        }

        public int GetPathLength()
        {
            return path.Count;
        }

        public List<Coordinates> GetPath()
        {
            return path;
        }

    }

    public class AntColonyAlgorithm
    {
        private Maze maze;
        private Ant[] ants;
        private double[,] pheromones;
        private double alpha;
        private double beta;
        private double evaporationRate;
        private double initialPheromone;
        private int antCount;
        private int iterations;

        public AntColonyAlgorithm(Maze maze, int antCount, int iterations, double alpha, double beta, double evaporationRate, double initialPheromone)
        {
            this.maze = maze;
            this.antCount = antCount;
            this.iterations = iterations;
            this.alpha = alpha;
            this.beta = beta;
            this.evaporationRate = evaporationRate;
            this.initialPheromone = initialPheromone;
            ants = new Ant[antCount];
            pheromones = new double[maze.GetRows(), maze.GetColumns()];
        }

        public void Run()
        {
            // Инициализация феромонов на всех путях
            for (int i = 0; i < maze.GetRows(); i++)
            {
                for (int j = 0; j < maze.GetColumns(); j++)
                {
                    pheromones[i, j] = initialPheromone;
                }
            }

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                // Создаем муравьев
                for (int antIndex = 0; antIndex < antCount; antIndex++)
                {
                    ants[antIndex] = new Ant(maze);
                }

                // Перемещаем муравьев до выхода из лабиринта
                for (int antIndex = 0; antIndex < antCount; antIndex++)
                {

                    while (!maze.IsExit(ants[antIndex].GetCurrentRow(), ants[antIndex].GetCurrentCol()))
                    {
                        // если путь больше, чем количество клеток в лабиринте, отбрасываем его
                        if (ants[antIndex].GetPathLength() > maze.GetColumns() * maze.GetRows())
                        {
                            break;
                        }
                        MoveAnt(antIndex);
                    }
                }

                // Обновляем феромоны на пройденных путях
                for (int i = 0; i < maze.GetRows(); i++)
                {
                    for (int j = 0; j < maze.GetColumns(); j++)
                    {
                        for (int antIndex = 0; antIndex < antCount; antIndex++)
                        {
                            if (ants[antIndex].IsVisited(i, j))
                            {
                                pheromones[i, j] = (1 - evaporationRate) * pheromones[i, j] + 1;
                            }
                            else
                            {
                                pheromones[i, j] *= (1 - evaporationRate);
                            }
                        }
                    }
                }

                // устанавливаем кратчайший путь в лабиринте
                maze.SetPath(GetShortestPath());
            }
        }

        private void MoveAnt(int antIndex)
        {
            // Выбираем следующую позицию на основе вероятности и эвристики
            (int nextRow, int nextCol) = ChooseNextPosition(ants[antIndex].GetCurrentRow(), ants[antIndex].GetCurrentCol());

            // Перемещаем муравья на следующую позицию
            ants[antIndex].MoveTo(nextRow, nextCol);
            ants[antIndex].SetVisited(nextRow, nextCol);
        }



        private (int, int) ChooseNextPosition(int row, int col)
        {
            int nextRow = -1;
            int nextCol = -1;
            double totalProbability = 0.0;
            double[,] probabilities = new double[maze.GetRows(), maze.GetColumns()];

            int startY = row == 0 ? 0 : row - 1;
            int startX = col == 0 ? 0 : col - 1;
            int endY = row == maze.GetRows() - 1 ? row : maze.GetRows() + 1;
            int endX = row == maze.GetColumns() - 1 ? col : maze.GetColumns() + 1;

            // Вычисляем вероятность для каждой соседней позиции
            for (int i = startY; i < endY; i++)
            {
                for (int j = startX; j < endX; j++)
                {
                    if (maze.IsWall(i, j) || ants.Any(ant => ant.GetCurrentRow() == i && ant.GetCurrentCol() == j))
                    {
                        // Если позиция - препятствие или уже посещена другим муравьем, игнорируем её
                        continue;
                    }

                    double pheromone = Math.Pow(pheromones[i, j], alpha);
                    double heuristic = Math.Pow(1.0 / (GetDistance(row, col, i, j) + 1.0), beta);
                    probabilities[i, j] = pheromone * heuristic;
                    totalProbability += probabilities[i, j];
                }
            }

            // Выбираем следующую позицию на основе вероятности
            double randomValue = new Random().NextDouble() * totalProbability;
            double currentProbability = 0.0;

            for (int i = 0; i < probabilities.GetLength(0); i++)
            {
                for (int j = 0; j < probabilities.GetLength(1); j++)
                {
                    if (probabilities[i, j] > 0)
                    {
                        currentProbability += probabilities[i, j];
                        if (currentProbability >= randomValue)
                        {
                            nextRow = i;
                            nextCol = j;
                            break;
                        }
                    }
                }

                if (nextRow != -1)
                {
                    break;
                }
            }

            if (maze.IsWall(nextRow, nextCol))
            {
                return (row, col);
            }

            return (nextRow, nextCol);
        }

        // Метод для вычисления эвристики - расстояния между позициями (здесь просто Манхэттенское расстояние)
        private int GetDistance(int row1, int col1, int row2, int col2)
        {
            return Math.Abs(row1 - row2) + Math.Abs(col1 - col2);
        }

        public List<Coordinates> GetShortestPath()
        {

            // устанавливаем самый длинный возможный путь
            int pathLength = maze.GetColumns() * maze.GetRows();
            List<Coordinates> result = new List<Coordinates>();

            for (int i = 0; i < antCount; i++)
            {
                if (ants[i].GetPathLength() < pathLength &&
                    maze.IsExit(ants[i].GetCurrentRow(), ants[i].GetCurrentCol()))
                {
                    pathLength = ants[i].GetPathLength();
                    result = ants[i].GetPath();
                }
            }
            return result;
        }

        public void Print()
        {
            maze.Print();
        }

        public void PrintWithPheromones()
        {
            maze.Print();
        }


    }
}
