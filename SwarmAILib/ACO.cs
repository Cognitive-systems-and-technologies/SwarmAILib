using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAILib
{
    public class Ant
    {
        private int currentRow;
        private int currentCol;
        private bool[,] visited;

        public Ant()
        {
            currentRow = 0;
            currentCol = 0;
        }

        public void MoveTo(int row, int col)
        {
            currentRow = row;
            currentCol = col;
        }

        public int GetCurrentRow()
        {
            return currentRow;
        }

        public int GetCurrentCol()
        {
            return currentCol;
        }

        public void SetVisited(int row, int col)
        {
            visited[row, col] = true;
        }

        public bool IsVisited(int row, int col)
        {
            return visited[row, col];
        }
    }

    public class AntColonyAlgorithm
    {
        private Maze maze;
        private Ant[] ants;
        private double[,] pheromones;
        private double alpha;
        private double beta;
        // скорость испарения феромона
        private double evaporationRate;
        // начальное значение феромона
        private double initialPheromone;
        // количество "муравьёв"
        private int antCount;
        // количество итераций алгоритма
        private int iterations;
        

        // конструктор класса
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
                    ants[antIndex] = new Ant();
                }

                // Перемещаем муравьев до выхода из лабиринта
                for (int antIndex = 0; antIndex < antCount; antIndex++)
                {
                    while (!maze.IsExit(ants[antIndex].GetCurrentRow(), ants[antIndex].GetCurrentCol()))
                    {
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

            // Вычисляем вероятность для каждой соседней позиции
            for (int i = 0; i < maze.GetRows(); i++)
            {
                for (int j = 0; j < maze.GetColumns(); j++)
                {
                    if (maze.IsWall(i, j) || ants.Any(ant => ant.GetCurrentRow() == i && ant.GetCurrentCol() == j))
                    {
                        // Если позиция - препятствие или уже посещена другим муравьем, игнорируем её
                        continue;
                    }

                    // вычисляем количество феромона в данной точке
                    double pheromone = Math.Pow(pheromones[i, j], alpha);
                    // вычисляем эвристику перехода в данную ячейку
                    double heuristic = Math.Pow(1.0 / (GetDistance(row, col, i, j) + 1.0), beta);
                    // вычисляем вероятность перехода в точку i, j
                    probabilities[i, j] = pheromone * heuristic;
                    totalProbability += probabilities[i, j];
                }
            }

            // Выбираем следующую позицию на основе вероятности
            double randomValue = new Random().NextDouble() * totalProbability;
            double currentProbability = 0.0;

            for (int i = 0; i < maze.GetRows(); i++)
            {
                for (int j = 0; j < maze.GetColumns(); j++)
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

            return (nextRow, nextCol);
        }

        // Метод для вычисления эвристики - расстояния между позициями (здесь просто Манхэттенское расстояние)
        private int GetDistance(int row1, int col1, int row2, int col2)
        {
            return Math.Abs(row1 - row2) + Math.Abs(col1 - col2);
        }

    }
}
