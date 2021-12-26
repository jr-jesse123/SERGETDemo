using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SERGETStore.App.Extentions;

public class MoedaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            var moeda = Convert.ToDecimal(value, new CultureInfo("pt-BR"));
        }
        catch (Exception)
        {
            return new ValidationResult("Moeda em formato inválido");
        }


        return ValidationResult.Success;
    }
}



public class MoedaAttributeAdpter : AttributeAdapterBase<MoedaAttribute>
{
    public MoedaAttributeAdpter(MoedaAttribute attribute, IStringLocalizer? stringLocalizer) : base(attribute, stringLocalizer)
    {
    }

    public override void AddValidation(ClientModelValidationContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context) );

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-moeda", GetErrorMessage(context));
        MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));


        //throw new NotImplementedException();
    }


    public override string GetErrorMessage(ModelValidationContextBase validationContext) => "Moeda em formato inválido";
 
}


public class MoedaValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly IValidationAttributeAdapterProvider baseProvider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
    {
        if (attribute is MoedaAttribute moedaAttribute)
        {
            return new MoedaAttributeAdpter(moedaAttribute, stringLocalizer);
        }

        return  baseProvider.GetAttributeAdapter(attribute, stringLocalizer); 
    }
}
