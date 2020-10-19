using DevIO.Business.Models.Validations.Documentos;
using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class FornecedorValidation : AbstractValidator<Fornecedor>
    {

        public FornecedorValidation()
        {
            //Validacoes para o campo nome
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            //Validacao condicional para pessoa fisica
            When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () => {
                
                //Validando tamanho do documento
                RuleFor(f => f.Documento.Length)
                    .Equal(CpfValidacao.TamanhoCpf).WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}");

                //Validando formato do documento
                RuleFor(f => CpfValidacao.Validar(f.Documento))
                    .Equal(true)
                    .WithMessage("O documento fornecido nao eh valido.");

            });

            //Validacao condicional para pessoa juridica
            When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () => {

                //Validando tamanho do documento
                RuleFor(f => f.Documento.Length)
                    .Equal(CnpjValidacao.TamanhoCnpj).WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}");

                //Validando formato do documento
                RuleFor(f => CnpjValidacao.Validar(f.Documento))
                    .Equal(true)
                    .WithMessage("O documento fornecido nao eh valido.");

            });

        }

    } //class

} //namespace
