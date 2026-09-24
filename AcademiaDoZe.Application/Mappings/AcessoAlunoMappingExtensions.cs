using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Application.Mappings;

public static class AcessoAlunoMappingExtensions
{
    public static AcessoAlunoDto ToDto(this AcessoAluno acesso)
    {
        ArgumentNullException.ThrowIfNull(acesso);

        return new AcessoAlunoDto
        {
            AlunoId = acesso.AlunoId,
            Aluno = acesso.Aluno,
            DataHora = acesso.DataHora
        };
    }

    public static AcessoAluno ToEntity(this AcessoAlunoDto acessoDto)
    {
        ArgumentNullException.ThrowIfNull(acessoDto);

        var result = AcessoAluno.Criar(
            acessoDto.AlunoId,
            acessoDto.Aluno,
            acessoDto.DataHora
        );

        if (result.IsFailure)
        {
            throw new InvalidOperationException(
                $"Erro de validação ao converter AcessoAluno: " +
                $"{string.Join(", ", result.Notifications.Select(n => n.Mensagem))}"
            );
        }

        return result.Value!;
    }

    public static AcessoAluno UpdateFromDto(
        this AcessoAluno acesso,
        AcessoAlunoDto acessoDto)
    {
        ArgumentNullException.ThrowIfNull(acesso);
        ArgumentNullException.ThrowIfNull(acessoDto);

        var result = AcessoAluno.Criar(
            acesso.Id,
            acessoDto.Aluno,
            acessoDto.DataHora
        );

        if (result.IsFailure)
        {
            throw new InvalidOperationException(
                $"Erro de validação ao atualizar AcessoAluno: " +
                $"{string.Join(", ", result.Notifications.Select(n => n.Mensagem))}"
            );
        }

        return result.Value!;
    }
}