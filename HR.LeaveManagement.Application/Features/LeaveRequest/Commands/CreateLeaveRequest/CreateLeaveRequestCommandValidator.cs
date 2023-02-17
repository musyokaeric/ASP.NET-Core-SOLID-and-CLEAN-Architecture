using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;

        public CreateLeaveRequestCommandValidator(ILeaveRequestRepository leaveRequestRepository)
        {
            this.leaveRequestRepository = leaveRequestRepository;

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(LeaveRequestMustExist)
                .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> LeaveRequestMustExist(int id, CancellationToken token)
        {
            return await leaveRequestRepository.GetByIdAsync(id) != null;
        }
    }
}
