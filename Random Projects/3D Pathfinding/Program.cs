using System;
using System.Collections.Generic;

namespace _3D_Pathfinding
{
    class Program
    {
        static void Main()
        {
            #region Boards  
            char[][][] board =
            {
                new char[][]
                {
                "█████████".ToCharArray(),
                "█  █    █".ToCharArray(),
                "█S █    █".ToCharArray(),
                "█████████".ToCharArray()
                },
                new char[][]
                {
                "█████████".ToCharArray(),
                "█   █   █".ToCharArray(),
                "█   █   █".ToCharArray(),
                "█████████".ToCharArray()
                },
                new char[][]
                {
                "█████████".ToCharArray(),
                "█     █ █".ToCharArray(),
                "█     █E█".ToCharArray(),
                "█████████".ToCharArray()
                }
            };
            #endregion
            try
            {
                // Warning: This will get stuck in corners :>
                var previousLocations = new List<(int X, int Y, int Z)>();
                (int X, int Y, int Z) targetPosition = (1, 1, 0);
                (int X, int Y, int Z) currentPosition = (1, 1, 0);
                FindStartAndEnd();
                DrawBoard();
                #region DijkstraPath algorithm 
                float closestValue = float.MaxValue;

                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Yellow;

                previousLocations.Add(currentPosition);
                while (currentPosition != targetPosition)
                {
                    foreach (var (X, Y, Z) in previousLocations)
                    {
                        foreach (int i in new int[] { 1, -1 })
                        {
                            CheckNeighbour(X + i, Y, Z);
                            CheckNeighbour(X, Y + i, Z);
                            CheckNeighbour(X, Y, Z + i);
                        }
                    }
                    if (!previousLocations.Contains(currentPosition))
                    {
                        previousLocations.Add(currentPosition);
                    }
                    board[currentPosition.Z][currentPosition.Y][currentPosition.X] = '█';
                }

                foreach (var (X, Y, Z) in previousLocations)
                {
                    Console.SetCursorPosition(X, (Y + 1) + Z * (board[0].Length + 1));
                    Console.WriteLine(":");
                }

                void CheckNeighbour(int X, int Y, int Z)
                {
                    float distanceFromTarget =
                        (((X - targetPosition.X) * (X - targetPosition.X)) +
                        ((Y - targetPosition.Y) * (Y - targetPosition.Y)) +
                        ((Z - targetPosition.Z) * (Z - targetPosition.Z))) / 2;

                    if (Z < 0 || Z >= board.Length ||
                        Y < 0 || Y >= board[Z].Length ||
                        X < 0 || X >= board[Z][Y].Length ||
                        board[Z][Y][X] is '█')
                    {
                        return;
                    }

                    if (Math.Abs(distanceFromTarget) <= closestValue)
                    {
                        currentPosition = (X, Y, Z);
                        closestValue = distanceFromTarget;
                    }
                }
                #endregion
                void FindStartAndEnd()
                {
                    for (int z = 0; z < board.Length; z++)
                    {
                        for (int y = 0; y < board[z].Length; y++)
                        {
                            for (int x = 0; x < board[z][y].Length; x++)
                            {
                                //Start
                                if (board[z][y][x] is 'S')
                                {
                                    currentPosition = (x, y, z);
                                }
                                //End
                                if (board[z][y][x] is 'E')
                                {
                                    targetPosition = (x, y, z);
                                }
                            }
                        }
                    }
                }
                void DrawBoard()
                {
                    for (int z = 0; z < board.Length; z++)
                    {
                        Console.WriteLine($" Floor {z + 1}");
                        for (int y = 0; y < board[z].Length; y++)
                        {
                            for (int x = 0; x < board[z][y].Length; x++)
                            {
                                Console.Write(board[z][y][x]);
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            finally
            {
                Console.CursorVisible = true;
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }
}
