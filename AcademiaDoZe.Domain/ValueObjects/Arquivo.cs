//Gabriel Francisco de Sousa
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Domain.ValueObjects;

public record Arquivo
{
    public string Valor { get; }

    private Arquivo(string valor)
    {
        Valor = valor;
    }
}