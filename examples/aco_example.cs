class Program
{
    static void Main(string[] args)
    {
        // Создаем новый лабиринт
        Maze maze = new Maze(10, 10);

        // Создаем новый экземпляр алгоритма колонии муравьев
        AntColonyAlgorithm antColonyAlgorithm = new AntColonyAlgorithm(maze, 100, 1000, 1.0, 1.0, 0.5, 1.0);

        // Запускаем алгоритм
        antColonyAlgorithm.Run();

        // Выводим результат алгоритма
        for (int i = 0; i < maze.GetRows(); i++)
        {
            for (int j = 0; j < maze.GetColumns(); j++)
            {
                Console.Write(antColonyAlgorithm.GetPheromoneLevel(i, j) + " ");
            }
            Console.WriteLine();
        }
    }
}