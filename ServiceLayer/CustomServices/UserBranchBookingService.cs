using DomainLayer.Models;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Identity;
using RepositoryLayer.IRepository;
using ServiceLayer.ICustomServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomServices
{
    public class UserBranchBookingService : IUserBranchBookingService
    {
        private readonly IRepository<UserBranchBooking> _userBranchBooking;
        private readonly IRepository<Branch> _branch;
        private readonly UserManager<IdentityUser> _userManager;
        public UserBranchBookingService(IRepository<UserBranchBooking> userBranchBooking, UserManager<IdentityUser> userManager, IRepository<Branch> branch)
        {
            _userBranchBooking = userBranchBooking;
            _userManager = userManager;
            _branch = branch;
        }

        public async Task<GeneralServiceResponse> GetAll(ComonParam param)
        {
            GeneralServiceResponse response = new GeneralServiceResponse();
            try
            {
                var dataQueryable = _branch.GetByCondition(x => x.IsDeleted != true &&
                   ((param.gloabalText == "null" || param.gloabalText == null) || (x.Title.Contains(param.gloabalText) || x.ManagerName.Contains(param.gloabalText))));
                response.totalCount = dataQueryable.Count();
                int firstPageLength = param.rows == 0 ? param.first : param.rows;
                response.data = dataQueryable.Skip(param.page * param.rows).Take(firstPageLength).OrderByDescending(x => x.CreatedDate).ToList().
                    Select(a => new BranchVM
                    {
                        Id = a.Id,
                        closingHourString = new DateTime(a.ClosingHour).ToString("hh:mm tt"),
                        openingHourString = new DateTime(a.OpeningHour).ToString("hh:mm tt"),
                        openingHour = new DateTime(a.OpeningHour).ToString(),
                        closingHour = new DateTime(a.ClosingHour).ToString(),
                        title = a.Title,
                        managerName = a.ManagerName,
                        canBook = _userBranchBooking.GetByCondition(x=>x.IsDeleted != true && x.User.Id == param.userId && x.Branch.Id == a.Id).FirstOrDefault() == null ? true : false 
                    }).
                    ToList();
                response.statusCode = HttpStatusCode.OK;
                response.success = true;
                response.message = "Success";
            }
            catch (Exception e)
            {
                response.totalCount = 0;
                response.data = null;
                response.message = e.Message;
                response.success = false;
                response.statusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
        public async Task<GeneralServiceResponse> Insert(UserBranchBookingVM entity)
        {

            GeneralServiceResponse response = new GeneralServiceResponse();
            try
            {
                if (entity != null)
                {
                    if (_userBranchBooking.GetByCondition(x => x.Branch.Id == entity.branchId && x.User.Id == entity.userId).FirstOrDefault() == null)
                    {
                        UserBranchBooking newUserBranchBooking = new UserBranchBooking();
                        newUserBranchBooking.Branch = new Branch();
                        newUserBranchBooking.Branch = _branch.GetByCondition(x=>x.Id == entity.branchId).FirstOrDefault();
                        newUserBranchBooking.User = new IdentityUser();
                        newUserBranchBooking.User = await _userManager.FindByIdAsync(entity.userId);
                        newUserBranchBooking.CreatedDate = DateTime.Now;
                        newUserBranchBooking.ModifiedDate = DateTime.Now;
                        newUserBranchBooking.IsDeleted = false;
                        _userBranchBooking.Insert(newUserBranchBooking);
                        _userBranchBooking.SaveChanges();
                        response.statusCode = HttpStatusCode.OK;
                        response.success = true;
                        response.message = "Success";
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.Found;
                        response.success = false;
                        response.message = "Already booked";
                    }
                }
            }
            catch (Exception e)
            {
                response.message = e.Message;
                response.success = false;
                response.statusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

    }
}