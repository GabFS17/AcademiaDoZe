//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Domain.ValueObjects;

public record Senha
{
    public string Valor { get; }
    private Senha(string valor)
    {
        Valor = valor;
    }
    public static Result<Senha> Criar(string valor)
    {
        if (NormalizacaoService.TextoVazioOuNulo(valor))
            return Result<Senha>.Failure("Senha", "SENHA_OBRIGATORIO");
        var textoLimpo = NormalizacaoService.LimparEspacos(valor);
        if (textoLimpo.Length < 6 || !textoLimpo.Any(char.IsUpper))
            return Result<Senha>.Failure("Senha", "SENHA_FORMATO");
        return Result<Senha>.Success(new Senha(textoLimpo));
    }
    public override string ToString() => Valor;
}