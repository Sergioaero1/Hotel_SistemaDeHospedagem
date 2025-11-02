using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_SistemaDeHospedagem.Models;

public class Pessoa
{
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string CPF { get; set; }

    public Pessoa(string nome, string sobrenome, string cpf)
    {
        Nome = nome;
        Sobrenome = sobrenome;
        CPF = cpf;
    }

    public string NomeCompleto => $"{Nome} {Sobrenome}";
}