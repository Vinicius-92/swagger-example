namespace SwaggerExample.Models;

public class Aluno
{
    /// <summary>
    /// Id do usuário
    /// </summary>
    /// <example>f03dae8d-6495-42b4-b524-efa8fadf30e8</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Número do RA
    /// </summary>
    /// <example>A4138679</example>
    public string? Ra { get; set; }
    
    /// <summary>
    /// Primeiro nome do aluno
    /// </summary>
    /// <example>Miguel</example>
    public string PrimeiroNome { get; set; } = string.Empty;
    
    /// <summary>
    /// Sobrenome do aluno
    /// </summary>
    /// <example>Silva</example>
    public string Sobrenome { get; set; }  = string.Empty;
    
    /// <summary>
    /// Número do CPF do aluno
    /// </summary>
    /// <example>31568198086</example>
    public string Cpf { get; set; }  = string.Empty;
    
    /// <summary>
    /// Idade do aluno
    /// </summary>
    /// <example>18</example>
    public int Idade { get; set; }
    
    /// <summary>
    /// Data de nascimento
    /// </summary>
    /// <example>1/1/0001 12:00:00 AM</example>
    public DateTime DataNascimento { get; set; }
}