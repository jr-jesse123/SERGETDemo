using FluentValidation;
using FluentValidation.Results;
using SERGETStore.Business.Interfaces;
using SERGETStore.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERGETStore.Business.Interfaces
{
    public abstract class BaseService
    {
        protected void Notificar(string mensagem)
        {
            // propagar erro até apresentação
        }

        protected void Notificar(ValidationResult validationResult)
        {
            validationResult.Errors.ForEach(e => Notificar(e.ErrorMessage));
        }

        protected bool ExecutarValidação<TV,TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid)
            {
                return true;
            }
            else
            {
                Notificar(validator);
                return false;
            }
        }
    }
}
