using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    enum BasicPlan
    {
        [Description("Північно-західний кут")]
        NorthWestAngle,
        [Description("Мінімальний елемент")]
        MinimalElement,
        [Description("Метод Фойгеля")]
        VogelMethod
    }
}
