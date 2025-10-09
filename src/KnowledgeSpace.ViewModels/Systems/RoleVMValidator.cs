using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.ViewModels.Systems
{
    public class RoleVMValidator : AbstractValidator<RoleVM>
    {
        public RoleVMValidator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id value is required")
                .MaximumLength(50).WithMessage("Role Id cannot over limit 50 characters");
            
            RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required");

        }
    }
}
