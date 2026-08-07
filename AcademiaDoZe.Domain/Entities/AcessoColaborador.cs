//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Domain.Entities;

public class AcessoColaborador : Entity
{
    public Colaborador Colaborador { get; private set; }
    public DateTime DataHoraChegada { get; private set; }
    public DateTime DataHoraSaida { get; private set; }

    private AcessoColaborador(int id, Colaborador colaborador, DateTime dataHoraChegada, DateTime dataHoraSaida) : base(id)
    {
        Colaborador = colaborador;
        DataHoraChegada = dataHoraChegada;
        DataHoraSaida = dataHoraSaida;
    }
    public static Result<AcessoColaborador> Criar(int id, Colaborador colaborador, DateTime dataHoraChegada, DateTime dataHoraSaida)
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

        // duração da jornada de trabalho: 8h - CLT, 6h Estagio
        if ((colaborador.Vinculo == ColaboradorVinculo.Clt && dataHoraChegada.AddHours(8) > dataHoraSaida)
            || (colaborador.Vinculo == ColaboradorVinculo.Estagio && dataHoraChegada.AddHours(6) > dataHoraSaida))
            notifications.Add(new Notification("Jornada", "DURACAO_JORNADA_TRABALHO_NAO_PERMITIDA"));

        if (notifications.Count != 0)
            return Result<AcessoColaborador>.Failure(notifications);

        var acessoColaborador = new AcessoColaborador(id, colaborador, dataHoraChegada, dataHoraSaida);
        return Result<AcessoColaborador>.Success(acessoColaborador);
    }
}
