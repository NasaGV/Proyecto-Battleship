using System;
using System.Collections.Generic;

namespace BattleShipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            bool juegoIniciado = false;

            while (true)
            {
                Console.WriteLine("¡Bienvenido!");
                Console.WriteLine("Menu ");
                Console.WriteLine("1. Reglas del juego ");
                Console.WriteLine("2. Comenzar Juego");
                Console.WriteLine("3. Salir");

                char opcionMenu = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (opcionMenu)
                {
                    case '1':
                        VerReglas();
                        break;
                    case '2':
                        juegoIniciado = true;
                        JugarBatallaNaval();
                        break;
                    case '3':
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
        }

        static void VerReglas()
        {

            Console.WriteLine("\nReglas del juego:");
            Console.WriteLine("- En el tablero se encuentran 2 submarinos, 3 barcos y 1 buzo:" +
                "Los submarinos ocupan 3 espacios en diagonal, los barcos 2 en horizontal y los buzos 1");
            Console.WriteLine("- El objetivo es acertar a estos objetivos enemigos ");
            Console.WriteLine("- Cada objetivo enemigo se colocara aleatoriamente en el tablero .");
            Console.WriteLine("- Si aciertas, se marcará con 'X' en los respectivos numeros de espacios que ocupa el objetivo. Si fallas, se marcará con 'O'.");
            Console.WriteLine("- Para dar alguna casilla del tablero deberas introducir el numero de fila y el numero de columna");
            Console.WriteLine("- Cada 5 turnos fallidos, obtendras un bonus donde podras llamar una taque aerio, solo si logras adivinar un numero entre 1-5");
            Console.WriteLine("- Si logras adivinar el numero y llamas el ataque aereo este impactara en una de las filas revelando lo que hay en ella. ");
            Console.WriteLine("Si la fila revelada con el ataque aereo contiene una sola x significa que le has dado a una parte del objetivo, este no marcara todas las casillas del objetivo.");
            Console.WriteLine("Presiona 'R' para regresar al menu principal.");
            while (Console.ReadKey().KeyChar != 'R') ;
            Console.Clear();
        }

        static void JugarBatallaNaval()
        {
            Console.Clear();
            Console.WriteLine("¡Comienza el juego de Batalla Naval!");
            Console.WriteLine();


            Tablero tableroJugador = new Tablero();

            Console.WriteLine("Coloque sus barcos:");


            tableroJugador.ColocarBarcos();

            Console.WriteLine("\n¡Comienza la batalla naval!\n");

            int intentosFallidos = 0; // Contador de intentos fallidos para el bonus
            int turnosSinAciertos = 0; // Contador de turnos sin aciertos para el ataque aéreo


            while (true)
            {

                tableroJugador.ImprimirTablero();


                Console.Write("Ingrese la fila para atacar (0-9): ");
                int x;
                if (!int.TryParse(Console.ReadLine(), out x) || x < 0 || x >= Tablero.TamañoTablero)
                {
                    Console.WriteLine("Por favor, ingrese una fila válida (0-9).");
                    continue;
                }

                Console.Write("Ingrese la columna para atacar (0-9): ");
                int y;
                if (!int.TryParse(Console.ReadLine(), out y) || y < 0 || y >= Tablero.TamañoTablero)
                {
                    Console.WriteLine("Por favor, ingrese una columna válida (0-9).");
                    continue;
                }

                string resultado = tableroJugador.Atacar(x, y);
                if (resultado != "")
                {
                    Console.WriteLine("¡Acertaste a un " + resultado + "!");
                    intentosFallidos = 0; // Reiniciar contador de intentos fallidos
                    turnosSinAciertos = 0; // Reiniciar contador de turnos sin aciertos
                }
                else
                {
                    Console.WriteLine("¡No hubo impacto! ¿Deseas marcar esta posición con una 'O'? (s/n)");
                    char eleccion = char.ToLower(Console.ReadKey().KeyChar);
                    if (eleccion == 's')
                    {
                        tableroJugador.MarcarFallo(x, y);
                        intentosFallidos++; // Incrementar contador de intentos fallidos
                        turnosSinAciertos++; // Incrementar contador de turnos sin aciertos

                        // Verificar si se activa el ataque aéreo cada 5 turnos sin aciertos
                        if (turnosSinAciertos % 5 == 0)
                        {
                            Console.WriteLine("\n¿Deseas lanzar un ataque aéreo? (s/n)");
                            char opcionAtaqueAereo = char.ToLower(Console.ReadKey().KeyChar);
                            if (opcionAtaqueAereo == 's')
                            {
                                Console.WriteLine("\n¡Has llamado al ataque aereo!");
                                if (tableroJugador.AdivinarNumeroParaAtaqueAereo())
                                {
                                    int filaAtaqueAereo = tableroJugador.LanzarAtaqueAereo();
                                    if (filaAtaqueAereo != -1)
                                    {
                                        Console.WriteLine($"El ataque aéreo ha sido lanzado en la fila {filaAtaqueAereo}.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No hay filas disponibles para el ataque aéreo. El ataque se cancela.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No has logrado adivinar el número. El ataque aéreo se cancela.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nHas decidido no llamar un ataque aéreo.");
                            }
                        }
                    }
                }


                if (tableroJugador.TodosLosBarcosHundidos())
                {
                    Console.WriteLine("\n¡Felicidades, has acertado a todos los objetivos!" +
                        "\n                     |\r\n                    |\r\n           |        |\r\n         |-|-|      |\r\n           |        |\r\n           | {O}    |\r\n           '--|     |\r\n             .|]_   |\r\n       _.-=.' |     |\r\n      |    |  |]_   |\r\n      |_.-='  |   __|__\r\n       _.-='  |\\   /|\\\r\n      |    |  |-'-'-'-'-.\r\n      |_.-='  '========='\r\n           `   |     |\r\n            `. |    / \\\r\n              ||   /   \\____.--=''''==--.._\r\n              ||_.'--=='    |__  __  __  _.'\r\n              ||  |    |    |\\ ||  ||  || |                        ___\r\n ____         ||__|____|____| \\||__||__||_/    __________________/|   |\r\n|    |______  |===.---. .---.========''''=-._ |     |     |     / |   |\r\n|    ||     |\\| |||   | |   |      '===' ||  \\|_____|_____|____/__|___|\r\n|-.._||_____|_\\___'---' '---'______....---===''======//=//////========|\r\n|--------------\\------------------/-----------------//-//////---------/\r\n|               \\                /                 // //////         /\r\n|                \\______________/                 // //////         /\r\n|                                        _____===//=//////=========/\r\n|=================================================================/\r\n'----------------------------------------------------------------`");
                    break;
                }
            }

            Console.WriteLine("Presiona 'R' para regresar al menu principal.");
            while (Console.ReadKey().KeyChar != 'R') ;
            Console.Clear();
        }
    }

    class Tablero
    {
        public const int TamañoTablero = 10;
        private char[,] barcos = new char[TamañoTablero, TamañoTablero];
        private char[,] impactos = new char[TamañoTablero, TamañoTablero];
        private HashSet<int> filasAtacadas = new HashSet<int>();

        public Tablero()
        {

            for (int i = 0; i < TamañoTablero; i++)
            {
                for (int j = 0; j < TamañoTablero; j++)
                {
                    barcos[i, j] = ' ';
                    impactos[i, j] = ' ';
                }
            }
        }

        public void ColocarBarcos()
        {
            Random aleatorio = new Random();

            for (int i = 0; i < 3; i++)
            {
                int x = aleatorio.Next(TamañoTablero);
                int y = aleatorio.Next(TamañoTablero - 2);
                while (barcos[x, y] != ' ' || barcos[x, y + 1] != ' ' || barcos[x, y + 2] != ' ')
                {
                    x = aleatorio.Next(TamañoTablero);
                    y = aleatorio.Next(TamañoTablero - 2);
                }

                barcos[x, y] = 'B';
                barcos[x, y + 1] = 'B';
            }


            for (int i = 0; i < 2; i++)
            {
                int x = aleatorio.Next(TamañoTablero - 2);
                int y = aleatorio.Next(TamañoTablero - 2);

                while (barcos[x, y] != ' ' || barcos[x + 1, y + 1] != ' ' || barcos[x + 2, y + 2] != ' ')
                {
                    x = aleatorio.Next(TamañoTablero - 2);
                    y = aleatorio.Next(TamañoTablero - 2);
                }

                barcos[x, y] = 'S';
                barcos[x + 1, y + 1] = 'S';
                barcos[x + 2, y + 2] = 'S';
            }


            int posX = aleatorio.Next(TamañoTablero);
            int posY = aleatorio.Next(TamañoTablero);
            while (barcos[posX, posY] != ' ')
            {
                posX = aleatorio.Next(TamañoTablero);
                posY = aleatorio.Next(TamañoTablero);
            }
            barcos[posX, posY] = 'P';
        }

        public string Atacar(int x, int y)
        {
            if (impactos[x, y] != ' ' && impactos[x, y] != 'X')
            {
                return "";
            }

            char objetivo = barcos[x, y];
            if (objetivo != ' ')
            {
                MarcarAcertado(x, y, objetivo);
                return objetivo == 'B' ? "barco" : (objetivo == 'S' ? "submarino" : "buzo");
            }
            else
            {
                impactos[x, y] = 'O';
                return "";
            }
        }

        private void MarcarAcertado(int x, int y, char objetivo)
        {
            if (objetivo == 'B')
            {

                impactos[x, y] = 'X';
                if (y < TamañoTablero - 1 && barcos[x, y + 1] == 'B')
                {
                    impactos[x, y + 1] = 'X';
                }
                else if (y > 0 && barcos[x, y - 1] == 'B')
                {
                    impactos[x, y - 1] = 'X';
                }
            }
            else if (objetivo == 'S')
            {
                for (int i = 0; i < 3; i++)
                {
                    if (x + i < TamañoTablero && y + i < TamañoTablero && barcos[x + i, y + i] == 'S')
                    {
                        impactos[x + i, y + i] = 'X';
                    }
                }
            }
            else if (objetivo == 'P')
            {
                impactos[x, y] = 'X';
            }
        }

        public void MarcarFallo(int x, int y)
        {
            impactos[x, y] = 'O';
        }

        public bool TodosLosBarcosHundidos()
        {
            for (int i = 0; i < TamañoTablero; i++)
            {
                for (int j = 0; j < TamañoTablero; j++)
                {
                    char cell = barcos[i, j];
                    if (cell != ' ' && impactos[i, j] != 'X')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void ImprimirTablero()
        {
            Console.WriteLine("\n  0 1 2 3 4 5 6 7 8 9 ");
            for (int i = 0; i < TamañoTablero; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < TamañoTablero; j++)
                {
                    if (impactos[i, j] == 'O' || impactos[i, j] == 'X')
                    {
                        Console.Write(impactos[i, j] + " ");
                    }
                    else
                    {
                        Console.Write(" " + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void RevelarFilaEntera(int fila)
        {
            for (int i = 0; i < TamañoTablero; i++)
            {
                if (barcos[fila, i] != ' ')
                {
                    impactos[fila, i] = 'X';
                }
                else
                {
                    impactos[fila, i] = 'O';
                }
            }
        }

        public bool AdivinarNumeroParaAtaqueAereo()
        {
            Random aleatorio = new Random();
            int numeroCorrecto = aleatorio.Next(1, 6);
            const int intentosMaximos = 3;
            int intentos = 0;

            while (intentos < intentosMaximos)
            {
                Console.Write($"Intento {intentos + 1}/{intentosMaximos}: Adivina un número del 1 al 5: ");
                int intentoUsuario;
                if (!int.TryParse(Console.ReadLine(), out intentoUsuario) || intentoUsuario < 1 || intentoUsuario > 5)
                {
                    Console.WriteLine("Por favor, ingrese un número válido del 1 al 5.");
                    continue;
                }

                if (intentoUsuario == numeroCorrecto)
                {
                    return true;
                }

                intentos++;
            }

            return false;
        }

        public int LanzarAtaqueAereo()
        {
            List<int> filasDisponibles = new List<int>();


            for (int i = 0; i < TamañoTablero; i++)
            {
                if (!filasAtacadas.Contains(i))
                {
                    filasDisponibles.Add(i);
                }
            }


            if (filasDisponibles.Count == 0)
            {
                return -1;
            }

            Random aleatorio = new Random();
            int filaAleatoria = filasDisponibles[aleatorio.Next(filasDisponibles.Count)];

            filasAtacadas.Add(filaAleatoria);

            RevelarFilaEntera(filaAleatoria);

            return filaAleatoria;
        }
    }
}