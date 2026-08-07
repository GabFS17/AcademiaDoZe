//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Domain.Entities;

public class AcessoAluno : Entity
{
    public Aluno Aluno { get; private set; }
    public DateTime DataHoraChegada { get; private set; }
    public DateTime DataHoraSaida { get; private set; }

    private AcessoAluno(int id, Aluno aluno, DateTime dataHoraChegada, DateTime dataHoraSaida) : base(id)
    {
        Aluno = aluno;
        DataHoraChegada = dataHoraChegada;
        DataHoraSaida = dataHoraSaida;
    }
    public static Result<AcessoAluno> Criar(int id, Aluno aluno, DateTime dataHoraChegada, DateTime dataHoraSaida)
    {
        var notifications = new List<Notification>();
        if (dataHoraChegada == default)
            notifications.Add(new Notification("DataHoraChegada", "DATA_HORA_CHEGADA_OBRIGATORIO"));
        else if (dataHoraChegada > DateTime.Now)
            notifications.Add(new Notification("DataHoraChegada", "DATA_HORA_CHEGADA_INVALIDA"));
        if (dataHoraSaida == default)
            notifications.Add(new Notification("DataHoraSaida", "DATA_HORA_SAIDA_OBRIGATORIO"));
        else if (dataHoraSaida > DateTime.Now)
            notifications.Add(new Notification("DataHoraSaida", "DATA_HORA_SAIDA_INVALIDA"));

        if (notifications.Count != 0)
            return Result<AcessoAluno>.Failure(notifications);

        var acessoAluno = new AcessoAluno(id, aluno, dataHoraChegada, dataHoraSaida);
        return Result<AcessoAluno>.Success(acessoAluno);
    }
    
}
