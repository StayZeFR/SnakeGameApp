using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SneakGameApp
{
    internal class Program
    {

        public static char snakeDirection;
        public static bool gameOverState = false;
        public static List<partBodySnake> snakeBody = new List<partBodySnake>();
        public static applePosition apple = new applePosition();

        public static Thread threadMovement = new Thread(new ThreadStart(movementSystem));
        public static Thread threadControl = new Thread(new ThreadStart(controlMovement));

        public class partBodySnake
        {
            public int xPos;
            public int yPos;
            public char partDirection;
        }

        public class applePosition
        {
            public int xPos;
            public int yPos;
        }
        public static void movementSystem()
        {
            while (true)
            {
                int xTemp = snakeBody[snakeBody.Count - 1].xPos;
                int yTemp = snakeBody[snakeBody.Count - 1].yPos;
                for (int i = snakeBody.Count - 1; i >= 0; i--)
                {
                    partBodySnake part = snakeBody[i];
                    if (i != 0) 
                    {
                        part.xPos = snakeBody[i - 1].xPos;
                        part.yPos = snakeBody[i - 1].yPos;
                        part.partDirection = snakeBody[i - 1].partDirection;
                    }
                    else
                    {
                        if (part.partDirection == 'u') part.yPos--;
                        else if (part.partDirection == 'd') part.yPos++;
                        else if (part.partDirection == 'l') part.xPos--;
                        else if (part.partDirection == 'r') part.xPos++;
                        if (part.xPos == apple.xPos && part.yPos == apple.yPos)
                        {
                            addBodyElement(snakeBody[snakeBody.Count - 1].xPos, snakeBody[snakeBody.Count - 1].yPos, snakeBody[snakeBody.Count - 1].partDirection);
                            addNewApple();
                        }
                        if (collisionCheck(part)) gameOver();
                    }
                    writerSystem(part.xPos, part.yPos, 'X');
                }
                writerSystem(xTemp, yTemp, ' ');
                Thread.Sleep(100);
            }
        }

        public static bool collisionCheck(partBodySnake part)
        {
            if (part.xPos == 0 || part.xPos == 59 || part.yPos == 0 || part.yPos == 29) return true;
            for (int i = 1; i < snakeBody.Count; i++)
            {
                if (part.xPos == snakeBody[i].xPos && part.yPos == snakeBody[i].yPos) return true;
            } 
            return false;
        }

        public static void gameOver()
        {
            writerSystem(1, 1, 'O');
            for (int h = 0; h < 30; h++)
            {
                for (int l = 0; l < 60; l++)
                {
                    Console.SetCursorPosition(l, h);
                    Console.Write("X");
                }
                Thread.Sleep(20);
            }
            for (int h = 0; h < 30; h++)
            {
                for (int l = 0; l < 60; l++)
                {
                    Console.SetCursorPosition(l, h);
                    Console.Write(" ");
                }
                Thread.Sleep(20);
            }
            Console.SetCursorPosition(0, 9);
            Thread.Sleep(200);
            Console.WriteLine("\t\t  _____                      ");
            Thread.Sleep(200);
            Console.WriteLine("\t\t / ____|                     ");
            Thread.Sleep(200);
            Console.WriteLine("\t\t| |  __  __ _ _ __ ___   ___ ");
            Thread.Sleep(200);
            Console.WriteLine("\t\t| | |_ |/ _` | '_ ` _ \\ / _ \\");
            Thread.Sleep(200);
            Console.WriteLine("\t\t| |__| | (_| | | | | | |  __/");
            Thread.Sleep(200);
            Console.WriteLine("\t\t \\_____|\\__,_|_| |_| |_|\\___|");
            Thread.Sleep(200);
            Console.WriteLine("\t\t    / __ \\                   ");
            Thread.Sleep(200);
            Console.WriteLine("\t\t   | |  | |_   _____ _ __    ");
            Thread.Sleep(200);
            Console.WriteLine("\t\t   | |  | \\ \\ / / _ \\ '__|   ");
            Thread.Sleep(200);
            Console.WriteLine("\t\t   | |__| |\\ V /  __/ |      ");
            Thread.Sleep(200);
            Console.WriteLine("\t\t    \\____/  \\_/ \\___|_|      ");
            Thread.Sleep(200);
            Console.ReadLine();
            threadMovement.Abort();
            threadControl.Abort();
        }

        public static void writerSystem(int x, int y, char c)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(c);
        }

        public static void addNewApple()
        {
            Random random = new Random();
            bool b;
            while (true)
            {
                b = false;
                int nX = random.Next(1, 59);
                int nY = random.Next(1, 29);
                snakeBody.ForEach(delegate (partBodySnake part)
                {
                    if (part.xPos == nX && part.yPos == nY) b = true;
                });
                if (!b)
                {
                    apple.xPos = nX;
                    apple.yPos = nY;
                    writerSystem(nX, nY, 'O');
                    break;
                }
            }
        }
        public static void controlMovement()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (snakeDirection != 'd') snakeDirection = 'u';
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (snakeDirection != 'u') snakeDirection = 'd';
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (snakeDirection != 'r') snakeDirection = 'l';
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (snakeDirection != 'l') snakeDirection = 'r';
                }
                snakeBody[0].partDirection = snakeDirection;
            }
        }

        public static void addBodyElement(int x, int y, char dir)
        {
            partBodySnake partBody = new partBodySnake();
            partBody.xPos = x;
            partBody.yPos = y;
            partBody.partDirection = dir;
            snakeBody.Add(partBody);
        }

        static void Main(string[] args)
        {

            Console.SetWindowSize(60, 30);
            Console.CursorVisible = false;
            Console.SetCursorPosition(Console.CursorLeft, 12);
            Thread.Sleep(500);
            Console.Write("   __          ________ _      _____ ____  __  __ ______ \r\n");
            Thread.Sleep(200);
            Console.Write("   \\ \\        / /  ____| |    / ____/ __ \\|  \\/  |  ____|\r\n");
            Thread.Sleep(200);
            Console.Write("    \\ \\  /\\  / /| |__  | |   | |   | |  | | \\  / | |__   \r\n");
            Thread.Sleep(200);
            Console.Write("     \\ \\/  \\/ / |  __| | |   | |   | |  | | |\\/| |  __|  \r\n");
            Thread.Sleep(200);
            Console.Write("      \\  /\\  /  | |____| |___| |___| |__| | |  | | |____ \r\n");
            Thread.Sleep(200);
            Console.Write("       \\/  \\/   |______|______\\_____\\____/|_|  |_|______|");
            Thread.Sleep(500);
            for (int h = 0; h < 30; h++)
            {
                for (int l = 0; l < 60; l++)
                {
                    Console.SetCursorPosition(l, h);
                    Console.Write("X");
                }
                Thread.Sleep(20);
            }
            for (int h = 0; h < 30; h++)
            {
                for (int l = 0; l < 60; l++)
                {
                    Console.SetCursorPosition(l, h);
                    if (h == 0 || h == 29) Console.Write("-");
                    else if (l == 0 || l == 59) Console.Write("|");
                    else Console.Write(" ");
                }
                Thread.Sleep(20);
            }

            writerSystem(10, 14, 'X');
            writerSystem(9, 14, 'X');
            writerSystem(8, 14, 'X');

            writerSystem(45, 14, 'O');

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) { snakeDirection = 'u'; break; }
                else if (key.Key == ConsoleKey.DownArrow) { snakeDirection = 'd'; break; }
                else if (key.Key == ConsoleKey.LeftArrow) { snakeDirection = 'l'; break; }
                else if (key.Key == ConsoleKey.RightArrow) { snakeDirection = 'r'; break; }
            }
            addBodyElement(10, 14, snakeDirection);
            addBodyElement(9, 14, 'r');
            addBodyElement(8, 14, 'r');
            apple.xPos = 45;
            apple.yPos = 14;

            Console.SetCursorPosition(1, 30);
            Console.WriteLine("- " + AppDomain.CurrentDomain.BaseDirectory);

            threadMovement.Start();
            threadControl.Start();
            
        }
    }
}
