using System;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixManipulator
{
    public static List<(int, int)> FindPathToMinimum(Vector3[][] matrix, int startRow, int startCol)
    {
        if (matrix == null || matrix.Length == 0)
            throw new ArgumentException("Matrix must not be null or empty");

        if (startRow < 0 || startRow >= matrix.Length ||
            startCol < 0 || startCol >= matrix[startRow].Length)
            throw new ArgumentException("Invalid starting position");

        var (targetRow, targetCol) = FindMinimumPosition(matrix);

        if (startRow == targetRow && startCol == targetCol)
            return new List<(int, int)> { (startRow, startCol) };

        var queue = new Queue<(int row, int col)>();
        var visited = new Dictionary<(int, int), bool>();
        var parent = new Dictionary<(int, int), (int, int)>();

        queue.Enqueue((startRow, startCol));
        visited[(startRow, startCol)] = true;

        bool found = false;

        while (queue.Count > 0)
        {
            var (currentRow, currentCol) = queue.Dequeue();

            if (currentRow == targetRow && currentCol == targetCol)
            {
                found = true;
                break;
            }

            foreach (var neighbor in GetValidNeighbors(matrix, currentRow, currentCol))
            {
                if (!visited.ContainsKey(neighbor))
                {
                    visited[neighbor] = true;
                    parent[neighbor] = (currentRow, currentCol);
                    queue.Enqueue(neighbor);
                }
            }
        }

        if (!found)
            throw new InvalidOperationException("No path exists to the minimum value");

        var path = new List<(int, int)>();
        var current = (targetRow, targetCol);
        while (current != (startRow, startCol))
        {
            path.Add(current);
            current = parent[current];
        }
        path.Add((startRow, startCol));
        path.Reverse();

        return path;
    }

    private static List<(int row, int col)> GetValidNeighbors(Vector3[][] matrix, int row, int col)
    {
        var neighbors = new List<(int, int)>();

        if (row > 0 && col < matrix[row - 1].Length)
            neighbors.Add((row - 1, col));

        if (row < matrix.Length - 1 && col < matrix[row + 1].Length)
            neighbors.Add((row + 1, col));

        if (col > 0)
            neighbors.Add((row, col - 1));

        if (col < matrix[row].Length - 1)
            neighbors.Add((row, col + 1));

        return neighbors;
    }

    private static (int row, int col) FindMinimumPosition(Vector3[][] matrix)
    {
        int minRow = 0;
        int minCol = 0;
        float minValue = matrix[0][0].y;

        for (int i = 0; i < matrix.Length; i++)
        {
            if (matrix[i] == null || matrix[i].Length == 0)
                throw new ArgumentException($"Row {i} is invalid");

            for (int j = 0; j < matrix[i].Length; j++)
            {
                if (matrix[i][j].y < minValue)
                {
                    minValue = matrix[i][j].y;
                    minRow = i;
                    minCol = j;
                }
            }
        }

        return (minRow, minCol);
    }

    public static int[] FindLargestRectangle(List<List<Vector3>> matrix, float threshold)
    {
        if (matrix == null || matrix.Count == 0 || matrix[0].Count == 0)
            return new int[0];

        int rows = matrix.Count;
        int cols = matrix[0].Count;
        int[] heights = new int[cols];
        int maxArea = 0;
        int topRow = -1, leftCol = -1, bottomRow = -1, rightCol = -1;

        for (int row = 0; row < rows; row++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[row][j].y <= threshold)
                    heights[j]++;
                else
                    heights[j] = 0;
            }

            Stack<int> stack = new Stack<int>();
            stack.Push(-1);

            for (int i = 0; i < cols; i++)
            {
                while (stack.Peek() != -1 && heights[stack.Peek()] >= heights[i])
                {
                    int mid = stack.Pop();
                    int h = heights[mid];
                    int left = stack.Peek();
                    int width = i - left - 1;

                    if (h == 0)
                        continue;

                    int area = h * width;

                    if (width != h && area > maxArea)
                    {
                        maxArea = area;
                        topRow = row - h + 1;
                        leftCol = left + 1;
                        bottomRow = row;
                        rightCol = i - 1;
                    }
                }
                stack.Push(i);
            }

            while (stack.Peek() != -1)
            {
                int mid = stack.Pop();
                int h = heights[mid];
                int left = stack.Peek();
                int width = cols - left - 1;

                if (h == 0)
                    continue;

                int area = h * width;

                if (width != h && area > maxArea)
                {
                    maxArea = area;
                    topRow = row - h + 1;
                    leftCol = left + 1;
                    bottomRow = row;
                    rightCol = cols - 1;
                }
            }
        }

        if (maxArea == 0)
        {
            return new int[0];
        }
        else
        {
            return new int[]
            {
                topRow, leftCol, bottomRow, rightCol
            };
        }
    }

    public static List<(int, int)> FindPathToLessThen(Vector3[][] matrix, int startRow, int startCol, float threshold)
    {
        if (matrix[startRow][startCol].y < threshold)
        {
            return new List<(int, int)> { (startRow, startCol) };
        }

        int[][] directions = new int[][]
        {
            new int[] { -1, 0 },
            new int[] { 1, 0 }, 
            new int[] { 0, -1 },
            new int[] { 0, 1 }
        };

        bool[][] visited = new bool[matrix.Length][];
        for (int i = 0; i < matrix.Length; i++)
        {
            visited[i] = new bool[matrix[i].Length];
        }

        Queue<Tuple<int, int, List<(int, int)>>> queue = new Queue<Tuple<int, int, List<(int, int)>>>();
        visited[startRow][startCol] = true;
        queue.Enqueue(Tuple.Create(startRow, startCol, new List<(int, int)> { (startRow, startCol) }));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int currentRow = current.Item1;
            int currentCol = current.Item2;
            List<(int, int)> currentPath = current.Item3;

            foreach (var dir in directions)
            {
                int newRow = currentRow + dir[0];
                int newCol = currentCol + dir[1];

                if (newRow >= 0 && newRow < matrix.Length)
                {
                    if (newCol >= 0 && newCol < matrix[newRow].Length && !visited[newRow][newCol])
                    {
                        visited[newRow][newCol] = true;
                        List<(int, int)> newPath = new List<(int, int)>(currentPath);
                        newPath.Add((newRow, newCol));

                        if (matrix[newRow][newCol].y < threshold)
                        {
                            return newPath;
                        }

                        queue.Enqueue(Tuple.Create(newRow, newCol, newPath));
                    }
                }
            }
        }

        return null;
    }
}