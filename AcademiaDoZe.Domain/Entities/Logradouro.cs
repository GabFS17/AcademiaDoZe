//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Domain.Entities;

public sealed class Logradouro : Entity
{
    public Cep Cep { get; protected set; }
    public string Nome { get; protected set; }
    public string Pais { get; protected set;  }
    public string Estado { get; protected set; }
    public string Cidade { get; protected set; }
    public string Bairro { get; protected set; }

    protected Logradouro(int id, Cep cep, string nome, string pais, string estado, string cidade, string bairro) : base(id)
    {
        Cep = cep;
        Nome = nome;
        Pais = pais;
        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
    }
    public static Result<Logradouro> Criar(int id, string cep, string nome, string bairro, string cidade, string estado, string pais)
    {
        var notifications = new List<Notification>();
        var cepResult = Cep.Criar(cep);
        if (cepResult.IsFailure)
            notifications.AddRange(cepResult.Notifications);
        if (NormalizadoService.TextoVazioOuNulo(nome))
            notifications.Add(new Notification("Nome", "NOME_OBRIGATORIO"));
        else
            nome = NormalizadoService.LimparEspacos(nome);
        if (NormalizadoService.TextoVazioOuNulo(bairro))
            notifications.Add(new Notification("Bairro", "BAIRRO_OBRIGATORIO"));
        else
            bairro = NormalizadoService.LimparEspacos(bairro);
        if (NormalizadoService.TextoVazioOuNulo(cidade))
            notifications.Add(new Notification("Cidade", "CIDADE_OBRIGATORIO"));
        else
            cidade = NormalizadoService.LimparEspacos(cidade);
        if (NormalizadoService.TextoVazioOuNulo(estado))
            notifications.Add(new Notification("Estado", "ESTADO_OBRIGATORIO"));
        else
            estado = NormalizadoService.ParaMaiusculo(NormalizadoService.LimparTodosEspacos(estado));
        if (NormalizadoService.TextoVazioOuNulo(pais))
            notifications.Add(new Notification("Pais", "PAIS_OBRIGATORIO"));
        else
            pais = NormalizadoService.LimparEspacos(pais);
        if (notifications.Count != 0)
            return Result<Logradouro>.Failure(notifications);
        return Result<Logradouro>.Success(new Logradouro(id, cepResult.Value!, nome, bairro, cidade, estado, pais));
    }
}
