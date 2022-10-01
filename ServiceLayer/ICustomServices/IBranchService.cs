using DomainLayer.Models;
using DomainLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ICustomServices
{
    public interface IBranchService
    {
        GeneralServiceResponse GetAll(ComonParam param);
        GeneralServiceResponse Insert(BranchVM entity);
        GeneralServiceResponse Update(BranchVM entity);
        GeneralServiceResponse Delete(long id);

    }
}
