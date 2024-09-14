using System;
using System.Collections.Generic;

class JogoDaVelhaPontuacao
{
    static char[] tabuleiro = { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
    static char jogador = 'O', computador = 'X';

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            ExibirTabuleiro();
            if (VerificarFimDeJogo())
                break;

            // Jogada do Computador
            Console.WriteLine("Jogada do Computador:");
            int jogadaComputador = EscolherMelhorJogada();
            tabuleiro[jogadaComputador] = computador;

            Console.Clear();
            ExibirTabuleiro();
            if (VerificarFimDeJogo())
                break;

            // Jogada do Jogador
            Console.Write("Escolha uma posição (0 a 8): ");
            int jogadaJogador = -1;
            bool jogadaValida = false;
            while (!jogadaValida)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out jogadaJogador) && jogadaJogador >= 0 && jogadaJogador <= 8 && tabuleiro[jogadaJogador] == ' ')
                {
                    jogadaValida = true;
                }
                else
                {
                    Console.Write("Jogada inválida. Escolha uma posição válida (0 a 8): ");
                }
            }
            tabuleiro[jogadaJogador] = jogador;
        }
    }

    static void ExibirTabuleiro()
    {
        Console.WriteLine(" {0} | {1} | {2} ", tabuleiro[0], tabuleiro[1], tabuleiro[2]);
        Console.WriteLine("---+---+---");
        Console.WriteLine(" {0} | {1} | {2} ", tabuleiro[3], tabuleiro[4], tabuleiro[5]);
        Console.WriteLine("---+---+---");
        Console.WriteLine(" {0} | {1} | {2} ", tabuleiro[6], tabuleiro[7], tabuleiro[8]);
    }

    static bool VerificarFimDeJogo()
    {
        int resultado = Avaliar(tabuleiro);
        if (resultado == 10)
        {
            Console.WriteLine("Computador venceu!");
            return true;
        }
        else if (resultado == -10)
        {
            Console.WriteLine("Jogador venceu!");
            return true;
        }
        else if (!ExisteMovimentosDisponiveis())
        {
            Console.WriteLine("Empate!");
            return true;
        }
        return false;
    }

    static int EscolherMelhorJogada()
    {
        List<int> posicoesDisponiveis = ObterPosicoesDisponiveis();
        int melhorPontuacao = int.MinValue;
        List<int> melhoresJogadas = new List<int>();

        foreach (int pos in posicoesDisponiveis)
        {
            int pontuacao = CalcularPontuacao(pos);
            // Console.WriteLine($"Posição {pos} -> Pontuação: {pontuacao}");
            if (pontuacao > melhorPontuacao)
            {
                melhorPontuacao = pontuacao;
                melhoresJogadas.Clear();
                melhoresJogadas.Add(pos);
            }
            else if (pontuacao == melhorPontuacao)
            {
                melhoresJogadas.Add(pos);
            }
        }

        // Se houver múltiplas jogadas com a mesma pontuação, escolher aleatoriamente
        Random rand = new Random();
        int indice = rand.Next(melhoresJogadas.Count);
        return melhoresJogadas[indice];
    }

    static int CalcularPontuacao(int pos)
    {
        int pontos = 0;

        // Mais 02 pontos se a posição for a central
        if (pos == 4)
            pontos += 2;

        // Mais 01 ponto se a posição estiver nos quatro cantos
        if (pos == 0 || pos == 2 || pos == 6 || pos == 8)
            pontos += 1;

        // Menos 02 pontos se já houver uma ou mais peças do adversário na mesma linha, coluna ou diagonal
        if (PosicaoConflitante(pos))
            pontos -= 2;

        // Mais 04 pontos se a posição impedir a vitória do adversário
        if (PodeBloquearVitoria(adversario: jogador, pos))
            pontos += 4;

        // Mais 04 pontos se a posição levar a uma vitória
        if (PodeVencer(pos))
            pontos += 4;

        return pontos;
    }

    static bool PosicaoConflitante(int pos)
    {
        // Verifica se há peças do adversário na mesma linha, coluna ou diagonal
        char adversario = jogador; // O jogador é o adversário do computador

        // Linhas
        int linha = pos / 3;
        for (int i = linha * 3; i < linha * 3 + 3; i++)
        {
            if (tabuleiro[i] == adversario)
                return true;
        }

        // Colunas
        int coluna = pos % 3;
        for (int i = coluna; i < 9; i += 3)
        {
            if (tabuleiro[i] == adversario)
                return true;
        }

        // Diagonais
        if (pos % 2 == 0) // Apenas as posições pares fazem parte das diagonais
        {
            // Diagonal principal
            bool diagonalPrincipal = true;
            for (int i = 0; i < 9; i += 4)
            {
                if (tabuleiro[i] == adversario)
                {
                    diagonalPrincipal = false;
                    break;
                }
            }
            if (diagonalPrincipal)
                return false;

            // Diagonal secundária
            bool diagonalSecundaria = true;
            for (int i = 2; i <= 6; i += 2)
            {
                if (tabuleiro[i] == adversario)
                {
                    diagonalSecundaria = false;
                    break;
                }
            }
            if (diagonalSecundaria)
                return false;

            return true;
        }

        return false;
    }

    static bool PodeBloquearVitoria(char adversario, int pos)
    {
        // Simula a jogada do adversário na posição 'pos' e verifica se isso leva à vitória
        tabuleiro[pos] = adversario;
        bool resultado = Avaliar(tabuleiro) == -10;
        tabuleiro[pos] = ' '; // Reverte a jogada
        return resultado;
    }

    static bool PodeVencer(int pos)
    {
        // Simula a jogada do computador na posição 'pos' e verifica se isso leva à vitória
        tabuleiro[pos] = computador;
        bool resultado = Avaliar(tabuleiro) == 10;
        tabuleiro[pos] = ' '; // Reverte a jogada
        return resultado;
    }

    static List<int> ObterPosicoesDisponiveis()
    {
        List<int> posicoes = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (tabuleiro[i] == ' ')
                posicoes.Add(i);
        }
        return posicoes;
    }

    static int Avaliar(char[] b)
    {
        // Verificar linhas
        for (int linha = 0; linha < 3; linha++)
        {
            if (b[linha * 3] == b[linha * 3 + 1] && b[linha * 3 + 1] == b[linha * 3 + 2])
            {
                if (b[linha * 3] == computador)
                    return 10;
                else if (b[linha * 3] == jogador)
                    return -10;
            }
        }

        // Verificar colunas
        for (int coluna = 0; coluna < 3; coluna++)
        {
            if (b[coluna] == b[coluna + 3] && b[coluna + 3] == b[coluna + 6])
            {
                if (b[coluna] == computador)
                    return 10;
                else if (b[coluna] == jogador)
                    return -10;
            }
        }

        // Verificar diagonais
        if (b[0] == b[4] && b[4] == b[8])
        {
            if (b[0] == computador)
                return 10;
            else if (b[0] == jogador)
                return -10;
        }

        if (b[2] == b[4] && b[4] == b[6])
        {
            if (b[2] == computador)
                return 10;
            else if (b[2] == jogador)
                return -10;
        }

        // Ninguém venceu
        return 0;
    }

    static bool ExisteMovimentosDisponiveis()
    {
        foreach (char c in tabuleiro)
        {
            if (c == ' ')
                return true;
        }
        return false;
    }
}
