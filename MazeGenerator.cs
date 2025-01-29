using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeRunners;

namespace MazeRunners
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var mazeGenerator = new MazeGenerator(50, 50);

            mazeGenerator.GenerateMaze();
            System.Console.WriteLine("Se genero el laberinto");
            mazeGenerator.PrintMaze();
            Console.ReadLine();
        }
    }
    public class MazeGenerator
    {
        private int ancho;
        private int altura;
        private Casilla[,] maze;
        private Random rand = new Random();
        private (int x,int y) winner;

        public MazeGenerator(int ancho, int altura)
        {
            this.ancho = ancho;
            this.altura = altura;
            maze = new Casilla[ancho, altura];

            winner = (ancho/2,altura/2);
            rand = new Random();
        }

        public void PrintMaze()
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    maze[i,j].Display();
                }
            }
            Console.WriteLine("Se imprimio el laberinto");
        }

        public Casilla[,] GenerateMaze()
        {
            for (int i = 0; i < ancho; i++)
            {
                for (int j = 0; j < altura; j++)
                {
                    if ( (i == 1 && j == 1) || (i == ancho - 2 && j == altura -2) || (i == 1 && j == altura - 2) || (i == ancho - 2 && j == 1))
                    {
                        maze[i,j] = new Camino((i,j));
                    }
                    else maze[i,j] = new Muro((i,j));
                }
            }
            Console.WriteLine("Laberinto generado con éxito de paredes.");

            AbrirCamino(maze, 1, 1, ancho - 2, altura - 2);
            Console.WriteLine("1 camino");

            AbrirCamino(maze, 1, altura - 2, ancho - 2, 1);
            System.Console.WriteLine("2 camino");

            ConectarCasilla(maze, winner);
            System.Console.WriteLine("victory");

            PonerTrampas();
            System.Console.WriteLine("trampas");

            return maze;
        }

        private void AbrirCamino(Casilla[,] maze, int i, int j, int o, int p)
        {
            if (maze[o,p] is Camino) 
            {
                maze[i,j] = new Camino((i,j)); 
                return;
            }

            List<(int,int)> dir = new List<(int,int)> {(1,0),(0,1),(-1,0),(0,-1)};
            if (IsBlock(i,j) && maze[i,j] is Muro)
            {
                foreach (var direction in dir)
                {
                    AbrirCamino(maze, i + direction.Item1, j + direction.Item2, o, p);
                }
            }
        }

        private void ConectarCasilla(Casilla[,] maze, (int x, int y) winner)
        {
            if (!(maze[winner.x,winner.y] is Camino))
            {
                maze[winner.x,winner.y] = new Camino(winner);
                return;
            }

            var dir = new List<(int, int)> {(0,1) , (1,0) , (0,-1) , (-1,0)};
            Shuffle(dir);

            foreach (var (dx, dy) in dir)
            {
                int nx = winner.x + dx;
                int ny = winner.y + dy;

                if (IsBlock(nx,ny) && maze[nx,ny] is Muro)
                {
                    maze[winner.x + dx, winner.y + dy] = new Camino((winner.x + dx/2, winner.y + dy/2));
                    break;
                }
            }
        }

        private void PonerTrampas()
        {
            int count = 10;

            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(1, ancho - 2);
                    y = rand.Next(1, altura - 2);
                } 
                while (!(maze[x,y] is Camino));

                int TipoTrampa = rand.Next(3);
                switch (TipoTrampa)
                {
                    case 0:
                        maze[x,y] = new SlowTrap((x,y));
                        break;
                    case 1:
                        maze[x,y] = new TeleportTrap((x,y));
                        break;
                    case 2:
                        maze[x,y] = new StunTrap((x,y));
                        break;
                }
            }
        }

        private bool IsBlock(int x, int y) => x > 0 && x < ancho && y > 0 && y < altura;

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;            
            }
        }
    }
}
