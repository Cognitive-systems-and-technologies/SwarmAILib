    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(10, 10);
            maze.Print();

            // Создаем новый экземпляр алгоритма колонии муравьев
            // некоторые лабиринты могут не иметь пути из точки входа в точку выхода, следует поэкспериментировать с параметрами модели
            AntColonyAlgorithm aco = new AntColonyAlgorithm(maze, 100, 100, 1.0, 1.0, 0.5, 0);
            aco.Run();
            aco.Print();

        }
    }