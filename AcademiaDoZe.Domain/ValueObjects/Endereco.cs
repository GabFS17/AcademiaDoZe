//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Domain.ValueObjects;

public record Endereco
{
    public Logradouro Logradouro { get; }
    public int Numero { get; }
    public string Complemento { get; }

    private Endereco(Logradouro logradouro, int numero, string complemento)
    {
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
    }
}