using FluentValidation.Attributes;
using ConsumerOne.Api.ViewModels.Validations;

namespace ConsumerOne.Api.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}