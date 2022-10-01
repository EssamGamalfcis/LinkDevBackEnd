using DomainLayer.Models;
using DomainLayer.ViewModels;
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
    public class BranchService : IBranchService
    {
        private readonly IRepository<Branch> _branchRepository;
        public BranchService(IRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }
        public GeneralServiceResponse GetAll(ComonParam param)
        {
            GeneralServiceResponse response = new GeneralServiceResponse();
            try
            {
                var dataQueryable = _branchRepository.GetByCondition(x=>x.IsDeleted != true &&
                   ((param.gloabalText == "null" || param.gloabalText == null) || (x.Title.Contains(param.gloabalText) || x.ManagerName.Contains(param.gloabalText))));
                response.totalCount = dataQueryable.Count();
                int firstPageLength = param.rows == 0 ? param.first : param.rows;
                response.data = dataQueryable.Skip(param.page * param.rows).Take(firstPageLength).OrderByDescending(x => x.CreatedDate).
                    Select(a => new BranchVM
                    {
                        Id = a.Id,
                        closingHourString = new DateTime(a.ClosingHour).ToString("hh:mm tt"),
                        openingHourString = new DateTime(a.OpeningHour).ToString("hh:mm tt"),
                        openingHour = new DateTime(a.OpeningHour).ToString(),
                        closingHour = new DateTime(a.ClosingHour).ToString(),
                        title = a.Title,
                        managerName = a.ManagerName
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
        public GeneralServiceResponse Insert(BranchVM entity)
        {
            GeneralServiceResponse response = new GeneralServiceResponse();
            try
            {
                if (entity != null)
                {
                    if (_branchRepository.GetByCondition(x => x.Title == entity.title && x.ManagerName == entity.managerName && x.IsDeleted != true).FirstOrDefault() == null)
                    {
                        if (DateTime.Parse(entity.closingHour).TimeOfDay > DateTime.Parse(entity.openingHour).TimeOfDay)
                        {
                            Branch newBranch = new Branch();
                            newBranch.ClosingHour = DateTime.Parse(entity.closingHour).TimeOfDay.Ticks;
                            newBranch.OpeningHour = DateTime.Parse(entity.openingHour).TimeOfDay.Ticks;
                            newBranch.Title = entity.title;
                            newBranch.ManagerName = entity.managerName;
                            newBranch.CreatedDate = DateTime.Now;
                            newBranch.ModifiedDate = DateTime.Now;
                            newBranch.IsDeleted = false;
                            _branchRepository.Insert(newBranch);
                            _branchRepository.SaveChanges();
                            response.statusCode = HttpStatusCode.OK;
                            response.success = true;
                            response.message = "Success";
                        }
                        else
                        {
                            response.statusCode = HttpStatusCode.Found;
                            response.success = false;
                            response.message = "Closing Hour field shall be less than the Opening Hour field.";
                        }
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.Found;
                        response.success = false;
                        response.message = "Already found";
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
        public GeneralServiceResponse Update(BranchVM entity)
        {
            GeneralServiceResponse response = new GeneralServiceResponse();
            try
            {
                if (entity != null)
                {
                    if (_branchRepository.GetByCondition(x => x.Title == entity.title && x.ManagerName == entity.managerName && x.Id != entity.Id && x.IsDeleted != true).FirstOrDefault() == null)
                    {
                        if (DateTime.Parse(entity.closingHour).TimeOfDay > DateTime.Parse(entity.openingHour).TimeOfDay)
                        {
                            Branch oldBranch = _branchRepository.GetByCondition(x => x.Id == entity.Id).FirstOrDefault();
                            if (oldBranch != null)
                            {
                                oldBranch.ClosingHour = DateTime.Parse(entity.closingHour).TimeOfDay.Ticks;
                                oldBranch.OpeningHour = DateTime.Parse(entity.openingHour).TimeOfDay.Ticks;
                                oldBranch.Title = entity.title;
                                oldBranch.ManagerName = entity.managerName;
                                oldBranch.ModifiedDate = DateTime.Now;
                                oldBranch.IsDeleted = false;
                                _branchRepository.Update(oldBranch);
                                _branchRepository.SaveChanges();
                                response.statusCode = HttpStatusCode.OK;
                                response.success = true;
                                response.message = "Success";
                            }
                        }
                        else
                        {
                            response.statusCode = HttpStatusCode.Found;
                            response.success = false;
                            response.message = "Closing Hour field shall be less than the Opening Hour field.";
                        }
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.NotFound;
                        response.success = false;
                        response.message = "Not found";
                    }
                }
                else
                {
                    response.statusCode = HttpStatusCode.Found;
                    response.success = false;
                    response.message = "Already found";
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
        public GeneralServiceResponse Delete(long id)
        {
            GeneralServiceResponse response = new GeneralServiceResponse();
            try
            {
                if (id != 0)
                {
                    Branch branchToDelete = _branchRepository.GetByCondition(x => x.Id == id).FirstOrDefault();
                    if (branchToDelete != null)
                    {
                        branchToDelete.IsDeleted = true;
                        branchToDelete.ModifiedDate = DateTime.Now;
                        _branchRepository.Update(branchToDelete);
                        _branchRepository.SaveChanges();
                        response.statusCode = HttpStatusCode.OK;
                        response.success = true;
                        response.message = "Success";
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.NotFound;
                        response.success = false;
                        response.message = "Not found";
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
