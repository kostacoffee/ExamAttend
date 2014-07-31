using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamAttend
{
	class UIBuilder
	{
		public static T buildUI<T>(Form f) {
			Control ui = (Control)Activator.CreateInstance(typeof(T));
			f.Controls.Add(ui);
			f.Size = ui.Size;
			return (T)Convert.ChangeType(ui, typeof(T));
			//http://stackoverflow.com/questions/972636/casting-a-variable-using-a-type-variable
		}

		public static GridViewForm buildUI(params string[] rows) {
			return new GridViewForm(rows);
		}
	}
}
