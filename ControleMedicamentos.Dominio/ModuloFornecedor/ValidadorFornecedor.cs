using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleMedicamentos.Dominio.Compartilhado;

namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class ValidadorFornecedor : AbstractValidator<Fornecedor>
    {
       public ValidadorFornecedor()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("Nome inválido!")
                .NotEmpty().WithMessage("Nome inválido!");

            RuleFor(x => x.Estado)
                .NotNull().WithMessage("Estado inválido!")
                .NotEmpty().WithMessage("Estado inválido!");

            RuleFor(x => x.Cidade)
                .NotNull().WithMessage("Cidade inválida!")
                .NotEmpty().WithMessage("Cidade inválida!");

            RuleFor(x => x.Telefone).Telefone()
                .NotEmpty().WithMessage("Telefone inválido")
                .NotNull().WithMessage("Telefone inválido!");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email inválido!");

        }

    }
}
