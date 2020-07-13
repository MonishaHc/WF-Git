using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_Tool.ClassLibrary
{
    public class Enum
    {

    }
    public enum RequestProcess // request header table process complete field
    {
        ForApprove = 0,
        ForProcessing = 1,
        ForEscalate = 2,
        Approved = 3,
        Processed = 4,
        Escalated = 5,
        Initiating = 6,
        APC_Process = 7,
        APC_Complete = 8,
        APP = 9,
        CLR = 10
    }
    public enum Request_Completing_Status
    {
        Initiating = 0,
        BeforeProcess = 1,
        AfterProcess = 2
    }

    public enum ClosingTypeOptions
    {
        CC = 0,// Close after Approvel Cycle Completes
        PC = 1,// Close by Processor
        AC = 2,// Auto Close after submission
        MC = 3// Close after all services are complete
    }
    public enum template_ActionFlag
    {
        ApproveProcess = 0,// In table is 1 
        Process = 1,// In table is 2
        Escalate = 2// In table is 3
    }

    public enum statusEnum
    {
        [Display(Name = "Select")]
        NewRequest = 0,
        [Display(Name = "Approve")]
        Approval = 1,
        [Display(Name = "APC")]
        APC = 2,
        APP = 3,
        REJ = 4,
        CLS = 5,
        HLD = 6,
        PYD = 7,
        PYR = 8,
        ESC = 9
    }
    public enum display_statusEnum // In the case of request insertion , we use the statusEnum, 
    { //but listing the request, we have to use the dispaly_statusEnum.
        All=0,//----------------------------------------
        [Display(Name = "Approval Pending")]
        Approval_Pending = 1,//----------------------INT Status with process 0
        Approved=2,//---------------------------INT Status with process 1
        [Display(Name = "Closing Pending")]
        Closing_Pending = 3, //--------------------APP with PC with process 0
        Closed=4,//-------------------------CLR 
        Paid=5//---------------------PYD
    }

    public enum OrganizationFlagEnum
    {
        DT=0,// Deaprtment table
        BL=1,// Business Line Table 
        B=2,// Business Table
        PG=3 // Product Group
    }

    public enum UserRole
    {
        Admin=0,
        Employee=1
    }

    public enum BooleanValue
    {
        No=0,
        Yes=1
    }
    public enum Template_Table_Action_Flag
    {
        // 07/04/2020 Alena
        //INT = 0,
        //APP = 1,
        //ESC = 2,
        //[Display(Name = "QIM/PIM")]
        //QIM = 3
        INT = 1,
        APP = 2,
        ESC = 3,
        [Display(Name = "QIM/PIM")]
        QIM = 1
    }
    public enum ServiceStatus
    {
        NotApplicable=0,
        Applied=1,
        Partial=2,
        Completed =3,
        Canceled=4
    }

    //Basheer on 12-12-2019 for role table
    public enum RoleTable
    {
        [Display(Name = "Business")]
        [Description("B")]
        B,
        [Display(Name = "BusinessLine")]
        [Description("BL")]
        BL,
        [Display(Name = "ProductGroup")]
        [Description("PG")]
        PG,
        [Display(Name = "Department")]
        [Description("DT")]
        DT

    }

    public enum ColumnName
    {
        [Display(Name = "Controller")]
        [Description("CR")]
        CR,
        [Display(Name = "Manager")]
        [Description("MN")]
        MN,
        [Display(Name = "OfficeAdmin")]
        [Description("OA")]
        OA
    }
    public enum YesorNo
    {
        Choose = 0,
        Yes = 1,
        No = 2
    }

    //--------------------------------
    public enum Location
    {
        East=0,
        West=1,
        Centre=2
    }

    public enum RequestType
    {
        Active=0,
        //Closed=1,
        //Cancelled=2,
        //Removed=4,
        //Rejected=5
        Others=1
    }
    public enum gender
    {
        Male=0,
        Female=1
    }
}
