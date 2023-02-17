using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests
{
    public class GetLeaveRequestListQuery : IRequest<List<LeaveRequestDto>>
    {
    }
}
