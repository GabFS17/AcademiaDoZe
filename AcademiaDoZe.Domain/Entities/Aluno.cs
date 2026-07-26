//Gabriel Francisco de Sousa
using System;
using System.Collections.Generic;
using System.Text;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities;

public class Aluno : Pessoa
{
    public Aluno(int id, string nome, Cpf cpf, Email email, Endereco endereco) : base(id, nome, cpf, email, endereco)
    {

    }
}