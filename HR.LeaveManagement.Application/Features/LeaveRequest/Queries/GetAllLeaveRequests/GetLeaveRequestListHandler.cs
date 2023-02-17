using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests
{
    public class GetLeaveRequestListHandler : IRequestHandler<GetLeaveRequestListQuery, List<LeaveRequestDto>>
    {
        private readonly IMapper mapper;
        private readonly ILeaveRequestRepository leaveRequestRepository;

        public GetLeaveRequestListHandler(IMapper mapper, ILeaveRequestRepository leaveRequestRepository)
        {
            this.mapper = mapper;
            this.leaveRequestRepository = leaveRequestRepository;
        }
        public async Task<List<LeaveRequestDto>> Handle(GetLeaveRequestListQuery request, CancellationToken cancellationToken)
        {
            var leaveRequests = await leaveRequestRepository.GetLeaveRequestsWithDetails();
            return mapper.Map<List<LeaveRequestDto>>(leaveRequests);
        }
    }
}
