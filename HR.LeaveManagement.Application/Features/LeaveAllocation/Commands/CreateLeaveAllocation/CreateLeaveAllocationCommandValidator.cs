using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandValidator : AbstractValidator<CreateLeaveAllocationCommand>
    {
        private readonly ILeaveAllocationRepository leaveAllocationRepository;

        public CreateLeaveAllocationCommandValidator(ILeaveAllocationRepository leaveAllocationRepository)
        {
            this.leaveAllocationRepository = leaveAllocationRepository;

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(LeaveTypeMustExist)
                .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
        {
            return await leaveAllocationRepository.GetByIdAsync(id) != null;
        }
    }
}
