using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class ValidadorMedicamento : AbstractValidator<Medicamento>
    {
        public ValidadorMedicamento()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("Nome inválido!")
                .NotEmpty().WithMessage("Nome inválido!");
            
            RuleFor(x => x.Descricao)
                .NotNull().WithMessage("Descrição inválida!")
                .NotEmpty().WithMessage("Descrição inválida!");
            
            RuleFor(x => x.Lote)
                .NotNull().WithMessage("Lote inválido!")
                .NotEmpty().WithMessage("Lote inválido!");
            
            RuleFor(x => x.QuantidadeDisponivel).GreaterThanOrEqualTo(0).WithMessage("Quantidade de medicamento inválida!");

            RuleFor(x => x.Validade)
                .NotNull().WithMessage("Validade inválida!")
                .NotEmpty().WithMessage("Validade inválida!")
                .GreaterThan(DateTime.Now).WithMessage("Validade inválida!");

            RuleFor(x => x.Fornecedor)
                .NotNull().WithMessage("Fornecedor inválido!")
                .NotEmpty().WithMessage("Fornecedor inválido!");    
           
        }
    }
}
