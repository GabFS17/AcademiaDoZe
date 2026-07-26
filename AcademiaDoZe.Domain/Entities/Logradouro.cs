//Gabriel Francisco de Sousa
using System;
using System.Collections.Generic;
using System.Text;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities;

public abstract class Logradouro : Entity
{
    public Cep Cep { get; protected set; }
    public string Pais { get; protected set;  }
    public string Estado { get; protected set; }
    public string Cidade { get; protected set; }
    public string Bairro { get; protected set; }
    public string NomeLogradouro { get; protected set; }

    protected Logradouro(int id, Cep cep, string pais, string estado, string cidade, string bairro, string nomeLogradouro) : base(id)
    {
        Cep = cep;
        Pais = pais;
        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
        NomeLogradouro = nomeLogradouro;
    }
}
