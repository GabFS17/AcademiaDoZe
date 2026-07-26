//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AcademiaDoZe.Domain.Enums;

namespace AcademiaDoZe.Domain.Entities;

public class Colaborador : Pessoa
{
    // encapsulamento das propriedades, aplicando imutabilidade
    public DateOnly DataAdmissao { get; private set; }
    public ColaboradorTipo Tipo { get; private set; }
    public ColaboradorVinculo Vinculo { get; private set; }

    // construtor privado para evitar instância direta 

    private Colaborador(int id,
                        string nome,
                        Cpf cpf,
                        DateOnly dataNascimento,
                        Telefone telefone,
                        Email email,
                        Endereco endereco,
                        Senha senha,
                        Arquivo foto,
                        DateOnly dataAdmissao,
                        ColaboradorTipo tipo,
                        ColaboradorVinculo vinculo)
        : base (id, nome, cpf, dataNascimento, telefone, email, senha, foto, endereco)
    {
        DataAdmissao = dataAdmissao;
        Tipo = tipo;
        Vinculo = vinculo;
    }
}