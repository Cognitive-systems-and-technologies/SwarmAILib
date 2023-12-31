﻿using System;

namespace SwarmAILib
{
    public struct Coordinates
    {
        public int x;
        public int y;
    }
    public class Maze
    {
        private int[,] grid;
        private int rows;
        private int columns;

        public Maze(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid = new int[rows, columns];
            GenerateRandomMaze();
        }

        private void GenerateRandomMaze()
        {
            Random random = new Random();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Задаем вероятность 30% для препятствия (значение 0)
                    grid[i, j] = random.Next(100) < 30 ? 0 : 1;
                }
            }

            // Обязательно убедимся, что вход и выход из лабиринта свободны
            grid[0, 0] = 1; // Вход
            grid[rows - 1, columns - 1] = 1; // Выход
        }

        public bool IsWall(int row, int col)
        {
            return row < 0 || row >= rows || col < 0 || col >= columns || grid[row, col] == 0;
        }

        public bool IsExit(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= columns)
            {
                throw new ArgumentOutOfRangeException("Invalid coordinates");
            }

            return (row == rows - 1 && col == columns - 1);
        }

        public int GetRows()
        {
            return rows;
        }

        public int GetColumns()
        {
            return columns;
        }

        public void SetPath(List<Coordinates> path)
        {
            foreach (Coordinates c in path)
            {
                grid[c.x, c.y] = -1;
            }
        }

        public void Print()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (grid[i, j] == 1)
                    {
                        Console.Write(" "); // свободный проход
                    }
                    else if (grid[i, j] == -1)
                    {
                        Console.Write("*"); // проход через лабиринт
                    }
                    else
                    {
                        Console.Write("#"); // препятствие
                    }
                }
                Console.Write("=");
                Console.WriteLine();
            }

            for (int i = 0; i <= columns; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
        }

    }




}