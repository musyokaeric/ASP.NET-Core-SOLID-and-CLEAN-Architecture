using AutoMapper;
using FluentValidation.Results;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        private readonly IMapper mapper;
        private readonly ILeaveTypeRepository leaveTypeRepository;
        private readonly IAppLogger<UpdateLeaveTypeCommandHandler> logger;

        public UpdateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository, 
            IAppLogger<UpdateLeaveTypeCommandHandler> logger)
        {
            this.mapper = mapper;
            this.leaveTypeRepository = leaveTypeRepository;
            this.logger = logger;
        }
        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate incoming data
            var validator = new UpdateLeaveTypeCommandValidator(leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);
            
            if (validationResult.Errors.Any()) 
            {
                logger.LogWarning($"Validation errors in update request for {nameof(LeaveType)} - {request.Id}");
                throw new BadRequestException("Invalid Leave type", validationResult);
            }

            var leaveType = mapper.Map<Domain.LeaveType>(request);

            // 2. Update database
            await leaveTypeRepository.UpdateAsync(leaveType);

            // 3. Return Unit value (nothing)
            return Unit.Value;
        }
    }
}
