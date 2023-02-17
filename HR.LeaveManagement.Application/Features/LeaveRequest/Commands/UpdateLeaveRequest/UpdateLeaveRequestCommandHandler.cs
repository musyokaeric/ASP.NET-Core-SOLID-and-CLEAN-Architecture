using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly IAppLogger<UpdateLeaveRequestCommandHandler> appLogger;
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly ILeaveTypeRepository leaveTypeRepository;

        public UpdateLeaveRequestCommandHandler(IMapper mapper, IEmailSender emailSender, IAppLogger<UpdateLeaveRequestCommandHandler> appLogger,
            ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository)
        {
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.appLogger = appLogger;
            this.leaveRequestRepository = leaveRequestRepository;
            this.leaveTypeRepository = leaveTypeRepository;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await leaveRequestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            var validator = new UpdateLeaveRequestCommandValidator(leaveRequestRepository, leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if(validationResult.Errors.Any()) 
            {
                throw new BadRequestException("Invalid Leave Allocation", validationResult);
            }

            await leaveRequestRepository.UpdateAsync(leaveRequest);

            try
            {
                // send confirmation email
                var email = new EmailMessage
                {
                    To = string.Empty, // Get email from employee record
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been updated successfullly.",
                    Subject = "Leave Request Updated"
                };
                await emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                appLogger.LogWarning(ex.Message);
                throw;
            }

            return Unit.Value;
        }
    }
}
