using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WF_TOOL.Controllers
{
    public class DataController : Controller
    {
        public object LoadApplicatonList(string domainid)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetAllApplication(domainid);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadWFTypeList(string applicationId)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetAllWFTypes(applicationId);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object GetAllBusinesscontrollersByCountry(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetAllBusinesscontrollersByCountry(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object GetAllBusinesslinecontrollersByBusiness(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetAllBusinesslinecontrollersByBusiness(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object GetAllPGcontrollersByBusinessline(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetAllPGcontrollersByBusinessline(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }

        public object GetAllProductGroupByProductGroupBusinessLine(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetAllPGByBusinessline(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object GetAllDepartmentsByProductGroup(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetAllDepartmentsByProductGroup(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }

        public object GetAllWFTypeByCountry(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetFullWFTypesByCountry(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        // Basherr  07/01/2020 - Start 
        public object WFTypeLoadBusinessByCountry(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetWFTypeLoadBusinessByCountry(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object WFTypeLoadBusinessLineByBusiness(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetWFTypeLoadBusinessLineByBusiness(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object WFTypeLoadApplicationByCountry(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetWFTypeLoadApplicationByCountry(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object WFTypeLoadEmployeeByCountry(string id)
        {
            var result = WF_Tool.DataLibrary.Data.DropdownData.GetWFTypeLoadEmployeeByCountry(id);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }

        // Basherr  07/01/2020 - End 

        /// <summary>
        /// Sibi 06-01-2020 ---GetEmployee_Location_Id_By_Cuntry
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>

        public object GetEmployee_Location_Id_By_Cuntry(string localEmplyee_ID)
        {

            var result = WF_Tool.DataLibrary.Data.DropdownData.GetEmployee_Location_Id_By_Cuntry(localEmplyee_ID);
            return Json(new { status = result.Distinct().ToList().Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }


        ///...................................................








    }
}