//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities;

public abstract class Pessoa : Entity
{
    public string Nome {  get; protected set; }
    public Cpf Cpf {  get; protected set; }
    public DateOnly DataNascimento { get; protected set; }
    public Telefone Telefone { get; protected set; }
    public Email Email { get; protected set; }
    public Senha Senha {  get; protected set; }
    public Arquivo Foto { get; protected set; }
    public Endereco Endereco { get; protected set; }

    public Pessoa(int id, string nome, Cpf cpf, DateOnly dataNascimento, Telefone telefone, Email email, Senha senha, Arquivo foto, Endereco endereco) : base(id)
    {
        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Email = email;
        Senha = senha;
        Foto = foto;
        Endereco = endereco;
    }
}
