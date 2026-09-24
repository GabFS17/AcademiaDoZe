using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Application.Mappings;

public static class AcessoColaboradorMappingExtensions
{
    public static AcessoColaboradorDto ToDto(this AcessoColaborador acesso)
    {
        ArgumentNullException.ThrowIfNull(acesso);

        return new AcessoColaboradorDto
        {
            ColaboradorId = acesso.ColaboradorId,
            Colaborador = acesso.Colaborador,
            DataHora = acesso.DataHora
        };
    }

    public static AcessoColaborador ToEntity(this AcessoColaboradorDto acessoDto)
    {
        ArgumentNullException.ThrowIfNull(acessoDto);

        var result = AcessoColaborador.Criar(
            acessoDto.ColaboradorId,
            acessoDto.Colaborador,
            acessoDto.DataHora
        );

        if (result.IsFailure)
        {
            throw new InvalidOperationException(
                $"Erro de validação ao converter AcessoColaborador: " +
                $"{string.Join(", ", result.Notifications.Select(n => n.Mensagem))}"
            );
        }

        return result.Value!;
    }

    public static AcessoColaborador UpdateFromDto(
        this AcessoColaborador acesso,
        AcessoColaboradorDto acessoDto)
    {
        ArgumentNullException.ThrowIfNull(acesso);
        ArgumentNullException.ThrowIfNull(acessoDto);

        var result = AcessoColaborador.Criar(
            acesso.Id,
            acessoDto.Colaborador,
            acessoDto.DataHora
        );

        if (result.IsFailure)
        {
            throw new InvalidOperationException(
                $"Erro de validação ao atualizar AcessoColaborador: " +
                $"{string.Join(", ", result.Notifications.Select(n => n.Mensagem))}"
            );
        }

        return result.Value!;
    }
}