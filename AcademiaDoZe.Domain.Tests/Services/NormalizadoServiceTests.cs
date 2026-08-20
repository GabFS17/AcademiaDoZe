using System;
using System.Collections.Generic;
using System.Text;

using AcademiaDoZe.Domain.Services;
namespace AcademiaDoZe.Domain.Tests.Services;

public class NormalizacaoServiceTests
{
    [Theory(DisplayName = "NormalizacaoService: TextoVazioOuNulo -> valida nulo/vazio")]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData("texto", false)]
    public void Deve_TextoVazioOuNulo_RetornarEsperado(string? input, bool expected)
    {
        var result = NormalizacaoService.TextoVazioOuNulo(input);
        Assert.Equal(expected, result);
    }
    [Theory(DisplayName = "NormalizacaoService: LimparEspacos -> normaliza espaços")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData(" a b c ", "a b c")]
    [InlineData("a\tb\nc", "a b c")]
    public void Deve_Normalizar_Espacos_Quando_LimparEspacosChamado(string? input, string expected)
    {
        var result = NormalizacaoService.LimparEspacos(input);
        Assert.Equal(expected, result);
    }
    [Theory(DisplayName = "NormalizacaoService: LimparTodosEspacos -> remove todos os espaços")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("a b c", "abc")]
    [InlineData(" a b ", "ab")]
    public void Deve_Remover_Todos_Espacos_Quando_LimparTodosEspacosChamado(string? input, string expected)
    {
        var result = NormalizacaoService.LimparTodosEspacos(input);
        Assert.Equal(expected, result);
    }
    [Theory(DisplayName = "NormalizacaoService: ParaMaiusculo -> converte para maiúsculo")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc", "ABC")]
    [InlineData("áéíõç", "ÁÉÍÕÇ")]
    public void Deve_Converter_Para_Maiusculo_Quando_ParaMaiusculoChamado(string? input, string expected)
    {
        var result = NormalizacaoService.ParaMaiusculo(input);
        Assert.Equal(expected, result);
    }
    [Theory(DisplayName = "NormalizacaoService: LimparEDigitos -> mantém apenas dígitos")]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("a1b2c3", "123")]
    [InlineData("(11) 91234-5678", "11912345678")]
    [InlineData("no-digits", "")]
    public void Deve_Manter_Apenas_Digitos_Quando_LimparEDigitosChamado(string? input, string expected)
    {
        var result = NormalizacaoService.LimparEDigitos(input);
        Assert.Equal(expected, result);
    }
    [Theory(DisplayName = "NormalizacaoService: TextoVazioOuNulo -> identifica diferentes tipos de entrada vazia")]
    [InlineData("\t", true)]
    [InlineData("\n", true)]
    [InlineData("\r\n", true)]
    [InlineData("   \t  ", true)]
    [InlineData("0", false)]
    public void Deve_Identificar_EntradaVazia_Quando_TextoVazioOuNuloChamado(string? input, bool expected)
    {
        var result = NormalizacaoService.TextoVazioOuNulo(input);

        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizacaoService: LimparEspacos -> remove espaços duplicados")]
    [InlineData("a  b", "a b")]
    [InlineData("  a  b  c  ", "a b c")]
    [InlineData("a\t\tb", "a b")]
    [InlineData("a\n\nb", "a b")]
    [InlineData("  texto   com   espaços  ", "texto com espaços")]
    public void Deve_Reduzir_Espacos_Quando_LimparEspacosChamado(string? input, string expected)
    {
        var result = NormalizacaoService.LimparEspacos(input);

        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizacaoService: LimparTodosEspacos -> remove tabs e quebras de linha")]
    [InlineData("a\tb", "ab")]
    [InlineData("a\nb", "ab")]
    [InlineData("a\r\nb", "ab")]
    [InlineData(" a \t b \n c ", "abc")]
    [InlineData("texto sem espaços", "textosemespaços")]
    public void Deve_Remover_Todos_Tipos_De_Espacos_Quando_LimparTodosEspacosChamado(string? input, string expected)
    {
        var result = NormalizacaoService.LimparTodosEspacos(input);

        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizacaoService: ParaMaiusculo -> mantém caracteres especiais em maiúsculo")]
    [InlineData("áéíóú", "ÁÉÍÓÚ")]
    [InlineData("ãõç", "ÃÕÇ")]
    [InlineData("João da Silva", "JOÃO DA SILVA")]
    [InlineData("teste@exemplo.com", "TESTE@EXEMPLO.COM")]
    [InlineData("abc123!@#", "ABC123!@#")]
    public void Deve_Converter_Corretamente_Para_Maiusculo_Com_CaracteresEspeciais(string? input, string expected)
    {
        var result = NormalizacaoService.ParaMaiusculo(input);

        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "NormalizacaoService: LimparEDigitos -> remove letras e caracteres especiais")]
    [InlineData("CPF: 123.456.789-00", "12345678900")]
    [InlineData("CEP: 80000-000", "80000000")]
    [InlineData("abc123!@#456", "123456")]
    [InlineData("123 456 789", "123456789")]
    [InlineData("Telefone: +55 (41) 99999-8888", "5541999998888")]
    public void Deve_Manter_Apenas_Digitos_Em_Diferentes_Formatos(string? input, string expected)
    {
        var result = NormalizacaoService.LimparEDigitos(input);

        Assert.Equal(expected, result);
    }
}