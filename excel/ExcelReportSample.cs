using ExcelReport;
using ExcelReport.Driver.NPOI;
using ExcelReport.Renderers;
using FHCollection.IBLL;
using FHCollection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHCollection.BLL
{
    public class CheckCompensationExcel
    {
        static CheckCompensationExcel()
        {
            // 项目启动时，添加
            Configurator.Put(".xls", new WorkbookLoader());
            Configurator.Put(".xlsx", new WorkbookLoader());
            
        }
        #region 导出核查表
        public static string ExportExcel(CheckCompensationExcelDataDTO data, ZS_Template template)
        {
            var toFileName = string.Format("{0}{1}{2}", data.ckCompDTO1.MemberName, data.ckCompDTO1.DescName, template.TemplateName);
            var getGuid = System.Guid.NewGuid().ToString().Replace("-", "");
            string tmpeSheetName = template.TemplateName;
            string getTemple = AppDomain.CurrentDomain.BaseDirectory + template.FilePath;
            string toSaveTemple = string.Format(@"{0}\App_Data\{1}_{2}.{3}", AppDomain.CurrentDomain.BaseDirectory, toFileName, getGuid, template.TemplateExtend);

            var getFlag = CacheHelper.GetCache<int>("excelFlagDemo", CacheConsts.config_ExcelFlag);
            if (getFlag <= 0)
            {
                getFlag = 0;
                CacheHelper.SetCache("excelFlagDemo", 0, CacheConsts.config_ExcelFlag);
            }
            string allFamilyCode = getFlag > 0 ? string.Format("{0}->{1}", data.ckCompDTO1.FamilyCode, DateTime.Now.toFormat()) : data.ckCompDTO1.FamilyCode;

            var listPara = new List<IElementRenderer>();

            var ckCompDTO1 = rrCheckMember1(data.ckCompDTO1, "1");
            listPara.AddRange(ckCompDTO1);

            var getSumProject5 = rrCheckMember5cale(data.getSumAreaProject, "5");
            listPara.AddRange(getSumProject5);

            ExportHelper.ExportToLocal(getTemple, toSaveTemple, ExcelReportsExts.SheetRendererExt(tmpeSheetName,
                                             listPara,
                                             rrCheckMember5(data.getMemberProject5.Where(a => a.AllArea.ToDouble() > 0).ToList(), "5"),
                                             rrCheckMember5(data.getMemberProject5.Where(a => a.EnjoymentArea7.ToDouble() > 0).ToList(), "51")
                                         ));
            return toSaveTemple;

        }
        #endregion
        #region 组装导出模板的数据

        public static List<IElementRenderer> rrCheckMember1(ZS_CheckCompensationDTO data, string prefix = "1")
        {
            if (data == null)
            {
                data = new ZS_CheckCompensationDTO();
            }
            var listPara = new List<IElementRenderer>();

            listPara.Add(new ParameterRenderer(prefix + "MemberName", data.MemberName));
            listPara.Add(new ParameterRenderer(prefix + "IDType", data.IDType));
            listPara.Add(new ParameterRenderer(prefix + "DescName", data.DescName));
            listPara.Add(new ParameterRenderer(prefix + "FamilyCode", data.FamilyCode));
            listPara.Add(new ParameterRenderer(prefix + "Mobile", data.Mobile));
            listPara.Add(new ParameterRenderer(prefix + "AllArea", data.AllArea.ToDouble()));
            listPara.Add(new ParameterRenderer(prefix + "ContractRegisterBuildingArea", data.ContractRegisterBuildingArea.ToDouble()));
            listPara.Add(new ParameterRenderer(prefix + "BuiltAreaAfterDate", data.BuiltAreaAfterDate.ToDouble()));
            listPara.Add(new ParameterRenderer(prefix + "BuiltAreaBeforeDate", data.BuiltAreaBeforeDate.ToDouble()));
            listPara.Add(new ParameterRenderer(prefix + "CollectArea1", data.CollectArea1.ToDouble()));
            listPara.Add(new ParameterRenderer(prefix + "CollectArea2", data.CollectArea2.ToDouble()));
            listPara.Add(new ParameterRenderer(prefix + "CollectArea3", data.CollectArea3.ToDouble()));
            listPara.Add(new ParameterRenderer(prefix + "CollectArea4", data.CollectArea4.ToDouble()));

            listPara.Add(new ParameterRenderer(prefix + "ViolationBuiltArea", data.ViolationBuiltArea.ToDouble()));


            return listPara;
        }

        public static List<IElementRenderer> rrCheckMember5cale(ZS_CheckMember_ProjectSumArea data, string prefix = "5")
        {
            if (data == null)
            {
                data = new ZS_CheckMember_ProjectSumArea();
            }
            var listPara = new List<IElementRenderer>();

            listPara.Add(new ParameterRenderer(prefix + "AllAreaSum", data.AllAreaSum));
            listPara.Add(new ParameterRenderer(prefix + "PurchaseAreaWorkNoSum", data.PurchaseAreaWorkNoSum));
            listPara.Add(new ParameterRenderer(prefix + "PurchaseAreaWorkSum", data.PurchaseAreaWorkSum));
            listPara.Add(new ParameterRenderer(prefix + "AllAreaEndSum", data.AllAreaEndSum));

            return listPara;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static RepeaterRenderer<ZS_CheckMember_ProjectDTO> rrCheckMember5(List<ZS_CheckMember_ProjectDTO> members, string prefix = "5")
        {
            //int allRow = 3;
            //if (members.Count < allRow)
            //{
            //    //加空行
            //    int toaddrow = allRow - members.Count;
            //    for (int i = 0; i < toaddrow; i++)
            //    {
            //        members.Add(new ZS_CheckMember_ProjectDTO() { Id = -1 });
            //    }
            //}
            if (prefix == "51")
            {
                return new RepeaterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "", members,
                            new ParameterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "MemberName", r => r.MemberName.NullToStr()),
                            new ParameterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "EnjoymentArea7", r => r.EnjoymentArea7.ToDouble())
                            );

            }
            return new RepeaterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "", members,
                            new ParameterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "MemberName", r => r.MemberName.NullToStr()),
                            new ParameterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "PurchaseAreaWork", r => r.PurchaseAreaWork.ToDouble()),
                            new ParameterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "PurchaseAreaWorkNo", r => r.PurchaseAreaWorkNo.ToDouble()),
                            //new ParameterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "EnjoymentArea7", r => r.EnjoymentArea7.NullToStr()),
                            new ParameterRenderer<ZS_CheckMember_ProjectDTO>(prefix + "AllArea", r => r.AllArea.ToDouble())
                            );
        }

        #endregion

    }


}
