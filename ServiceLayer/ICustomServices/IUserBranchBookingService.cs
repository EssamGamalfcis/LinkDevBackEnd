using DomainLayer.Models;
using DomainLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ICustomServices
{
    public interface IUserBranchBookingService
    {
        Task<GeneralServiceResponse> Insert(UserBranchBookingVM entity);
        Task<GeneralServiceResponse> GetAll(ComonParam param);
    }
}
