using ExcelReport.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReport.Renderers
{
    public static class ExcelReportsExts
    {

        public static SheetRenderer SheetRendererExt(string sheetName, List<IElementRenderer> parameters, params IElementRenderer[] elementRenderers)
        {
            parameters.AddRange(elementRenderers);
            return new SheetRenderer(sheetName, parameters.ToArray());
        }
    }
}
