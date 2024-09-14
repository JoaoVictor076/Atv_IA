using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{



     class JogoVelhaMinMax
    {
        static char[] tabuleiro = { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
        static char jogador = 'X', adversario = 'O';

        static void Main(string[] args)
        {
            // Exemplo de execução de uma partida com jogadas automáticas e manuais.
            while (true)
            {
                Console.Clear();
                ExibirTabuleiro();
                if (FimDeJogo())
                    break;

                if (jogador == 'X')
                {
                    int melhorJogada = MelhorJogada();
                    tabuleiro[melhorJogada] = jogador;
                    jogador = adversario;
                }
                else
                {
                    Console.WriteLine("Escolha uma posição (0 a 8): ");
                    int escolha = int.Parse(Console.ReadLine());
                    if (tabuleiro[escolha] == ' ')
                    {
                        tabuleiro[escolha] = jogador;
                        jogador = 'X';
                    }
                }
            }
        }

        static void ExibirTabuleiro()
        {
            Console.WriteLine($"{tabuleiro[0]} | {tabuleiro[1]} | {tabuleiro[2]}");
            Console.WriteLine("---------");
            Console.WriteLine($"{tabuleiro[3]} | {tabuleiro[4]} | {tabuleiro[5]}");
            Console.WriteLine("---------");
            Console.WriteLine($"{tabuleiro[6]} | {tabuleiro[7]} | {tabuleiro[8]}");
        }

        static bool FimDeJogo()
        {
            int resultado = Avaliar(tabuleiro);
            if (resultado == 10)
            {
                Console.WriteLine("Jogador 'X' venceu!");
                return true;
            }
            else if (resultado == -10)
            {
                Console.WriteLine("Jogador 'O' venceu!");
                return true;
            }
            else if (!ExisteMovimentosDisponiveis(tabuleiro))
            {
                Console.WriteLine("Empate!");
                return true;
            }
            return false;
        }

        static int MelhorJogada()
        {
            int melhorValor = -1000;
            int melhorMovimento = -1;

            for (int i = 0; i < 9; i++)
            {
                if (tabuleiro[i] == ' ')
                {
                    tabuleiro[i] = jogador;
                    int valorMovimento = Minimax(tabuleiro, 0, false);
                    tabuleiro[i] = ' ';

                    if (valorMovimento > melhorValor)
                    {
                        melhorMovimento = i;
                        melhorValor = valorMovimento;
                    }
                }
            }
            return melhorMovimento;
        }

        static int Minimax(char[] tabuleiro, int profundidade, bool isMax)
        {
            int placar = Avaliar(tabuleiro);

            if (placar == 10)
                return placar - profundidade;

            if (placar == -10)
                return placar + profundidade;

            if (!ExisteMovimentosDisponiveis(tabuleiro))
                return 0;

            if (isMax)
            {
                int melhor = -1000;

                for (int i = 0; i < 9; i++)
                {
                    if (tabuleiro[i] == ' ')
                    {
                        tabuleiro[i] = jogador;
                        melhor = Math.Max(melhor, Minimax(tabuleiro, profundidade + 1, !isMax));
                        tabuleiro[i] = ' ';
                    }
                }
                return melhor;
            }
            else
            {
                int melhor = 1000;

                for (int i = 0; i < 9; i++)
                {
                    if (tabuleiro[i] == ' ')
                    {
                        tabuleiro[i] = adversario;
                        melhor = Math.Min(melhor, Minimax(tabuleiro, profundidade + 1, !isMax));
                        tabuleiro[i] = ' ';
                    }
                }
                return melhor;
            }
        }

        static int Avaliar(char[] b)
        {
            // Verificar linhas
            for (int linha = 0; linha < 3; linha++)
            {
                if (b[linha * 3] == b[linha * 3 + 1] && b[linha * 3 + 1] == b[linha * 3 + 2])
                {
                    if (b[linha * 3] == jogador)
                        return 10;
                    else if (b[linha * 3] == adversario)
                        return -10;
                }
            }

            // Verificar colunas
            for (int coluna = 0; coluna < 3; coluna++)
            {
                if (b[coluna] == b[coluna + 3] && b[coluna + 3] == b[coluna + 6])
                {
                    if (b[coluna] == jogador)
                        return 10;
                    else if (b[coluna] == adversario)
                        return -10;
                }
            }

            // Verificar diagonais
            if (b[0] == b[4] && b[4] == b[8])
            {
                if (b[0] == jogador)
                    return 10;
                else if (b[0] == adversario)
                    return -10;
            }

            if (b[2] == b[4] && b[4] == b[6])
            {
                if (b[2] == jogador)
                    return 10;
                else if (b[2] == adversario)
                    return -10;
            }

            // Ninguém venceu
            return 0;
        }

        static bool ExisteMovimentosDisponiveis(char[] tabuleiro)
        {
            for (int i = 0; i < 9; i++)
            {
                if (tabuleiro[i] == ' ')
                    return true;
            }
            return false;
        }
    }

}

