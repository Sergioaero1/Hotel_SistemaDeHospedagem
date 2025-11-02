using System;
using System.Collections.Generic;
using Hotel_SistemaDeHospedagem.Models;

class Program
{
    static void Main()
    {
        while (true)
        {
            var tiposValidos = new List<string> { "premium", "luxo", "simples" };

            Console.WriteLine("Digite o tipo da suíte (Premium, Luxo ou Simples):");
            string? tipoSuiteInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(tipoSuiteInput)) continue;
            string tipoSuite = tipoSuiteInput.Trim().ToLower();
            if (!tiposValidos.Contains(tipoSuite))
            {
                Console.WriteLine("Tipo de suíte inválido. Tente novamente.\n");
                continue;
            }

            Console.WriteLine("Digite a capacidade da suíte:");
            if (!int.TryParse(Console.ReadLine(), out int capacidade) || capacidade <= 0)
            {
                Console.WriteLine("Capacidade inválida. Tente novamente.\n");
                continue;
            }

            Console.WriteLine("Digite o valor da diária:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal valorDiaria) || valorDiaria <= 0)
            {
                Console.WriteLine("Valor da diária inválida. Tente novamente.\n");
                continue;
            }

            Console.WriteLine("Quantos dias de reserva?");
            if (!int.TryParse(Console.ReadLine(), out int dias) || dias <= 0)
            {
                Console.WriteLine("Quantidade de dias inválida. Tente novamente.\n");
                continue;
            }

            Console.WriteLine("Quantos hóspedes?");
            if (!int.TryParse(Console.ReadLine(), out int qtdHospedes) || qtdHospedes <= 0)
            {
                Console.WriteLine("Quantidade de hóspedes inválida. Tente novamente.\n");
                continue;
            }

            var suite = new Suite(Capitalizar(tipoSuite), capacidade, valorDiaria);
            var reserva = new Reserva();
            reserva.CadastrarSuite(suite);
            reserva.CadastrarDias(dias);

            var hospedes = new List<Pessoa>();

            for (int i = 0; i < qtdHospedes; i++)
            {
                Console.WriteLine($"Digite o nome do hóspede {i + 1}:");
                string? nome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nome)) continue;

                Console.WriteLine($"Digite o sobrenome do hóspede {i + 1}:");
                string? sobrenome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(sobrenome)) continue;

                Console.WriteLine($"Digite o CPF do hóspede {i + 1} (somente números):");
                string? cpfInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cpfInput) || !long.TryParse(cpfInput, out _) || cpfInput.Length != 11)
                {
                    Console.WriteLine("⚠️ CPF inválido. Deve conter exatamente 11 números.");
                    Console.WriteLine("Deseja reiniciar o cadastro ou encerrar? (r/e)");
                    string? decisao = Console.ReadLine();
                    if (decisao?.Trim().ToLower() == "r")
                    {
                        Console.WriteLine("\nReiniciando cadastro...\n");
                        goto Reiniciar;
                    }
                    else
                    {
                        Console.WriteLine("\nEncerrando o sistema...");
                        return;
                    }
                }

                hospedes.Add(new Pessoa(nome, sobrenome, cpfInput));
            }

            try
            {
                if (hospedes.Count == 0)
                {
                    Console.WriteLine("⚠️ Erro: Não é possível reservar uma suíte sem hóspedes. Tente novamente.\n");
                    continue;
                }

                reserva.CadastrarHospedes(hospedes);

                Console.WriteLine("\nConfirme os dados da reserva:");
                Console.WriteLine($"Suíte: {suite.TipoSuite}");
                Console.WriteLine($"Capacidade: {suite.Capacidade}");
                Console.WriteLine($"Valor da diária: R${suite.ValorDiaria:F2}");
                Console.WriteLine($"Dias reservados: {dias}");
                Console.WriteLine($"Hóspedes ({reserva.ObterQuantidadeHospedes()}):");

                foreach (var h in hospedes)
                {
                    Console.WriteLine($"- {h.NomeCompleto} | CPF: {h.CPF}");
                }

                decimal valorTotal = reserva.CalcularValorDiaria();
                Console.WriteLine($"Valor total: R${valorTotal:F2}");

                Console.WriteLine("\nOs dados estão corretos? (s/n)");
                string? confirmacao = Console.ReadLine();
                if (confirmacao?.Trim().ToLower() != "s")
                {
                    Console.WriteLine("\nReiniciando cadastro...\n");
                    continue;
                }

                Console.WriteLine("\nEscolha a forma de pagamento:");
                Console.WriteLine("C - Cartão de Crédito");
                Console.WriteLine("D - Cartão de Débito");
                Console.WriteLine("P - Pix");
                string? formaPagamento = Console.ReadLine()?.Trim().ToLower();

                decimal valorFinal = valorTotal;

                if (formaPagamento == "c")
                {
                    Console.WriteLine("Em quantas parcelas deseja pagar? (1 a 12):");
                    if (!int.TryParse(Console.ReadLine(), out int parcelas) || parcelas < 1 || parcelas > 12)
                    {
                        Console.WriteLine("Número de parcelas inválido. Reiniciando cadastro...\n");
                        continue;
                    }

                    if (parcelas > 3)
                    {
                        valorFinal *= 1.10m;
                        Console.WriteLine($"Pagamento em {parcelas}x COM juros de 10%.");
                    }
                    else
                    {
                        Console.WriteLine($"Pagamento em {parcelas}x SEM juros.");
                    }

                    Console.WriteLine($"Valor final: R${valorFinal:F2} ({parcelas}x de R${valorFinal / parcelas:F2})");
                }
                else if (formaPagamento == "d" || formaPagamento == "p")
                {
                    if (dias < 11)
                    {
                        valorFinal *= 0.95m;
                        Console.WriteLine("Pagamento à vista com 5% de desconto aplicado.");
                    }
                    else
                    {
                        Console.WriteLine("Pagamento à vista sem desconto (reserva acima de 10 dias).");
                    }

                    Console.WriteLine($"Valor final: R${valorFinal:F2}");
                }
                else
                {
                    Console.WriteLine("Forma de pagamento inválida. Reiniciando cadastro...\n");
                    continue;
                }

                Console.WriteLine("\nPagamento feito com sucesso!");
                Console.WriteLine("Obrigado pela preferência, desfrutem da hospedagem na AERO HOTEL!");
                Console.WriteLine("Pressione ENTER para encerrar...");
                Console.ReadLine();
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}\nReiniciando cadastro...\n");
            }

        Reiniciar:;
        }
    }

    static string Capitalizar(string texto)
    {
        return char.ToUpper(texto[0]) + texto.Substring(1).ToLower();
    }
}
