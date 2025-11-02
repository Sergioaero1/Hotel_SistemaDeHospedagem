using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Hotel_SistemaDeHospedagem.Models;

public class Reserva
{
    public List<Pessoa> Hospedes { get; private set; } = new();
    public Suite? Suite { get; private set; }
    public int DiasReservados { get; private set; }

    public void CadastrarHospedes(List<Pessoa> hospedes)
    {
        if (Suite == null || hospedes.Count > Suite.Capacidade)
        {
            throw new Exception("Capacidade da suíte excedida.");
        }

        Hospedes = hospedes;
    }

    public void CadastrarSuite(Suite suite)
    {
        Suite = suite;
    }

    public void CadastrarDias(int dias)
    {
        DiasReservados = dias;
    }

    public int ObterQuantidadeHospedes()
    {
        return Hospedes.Count;
    }

    public decimal CalcularValorDiaria()
    {
        if (Suite == null) return 0;
        decimal valor = DiasReservados * Suite.ValorDiaria;
        return DiasReservados > 10 ? valor * 0.9m : valor;
    }
}