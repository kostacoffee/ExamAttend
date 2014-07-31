using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExamAttend
{
    public class ExtendedDataGridView : DataGridView
    {
        delegate void remove<T>(T item); 

        public ExtendedDataGridView() : base() {
            this.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        public void removeRecord<T>(T record)
        {
            for (int i = 0; i < this.Rows.Count; i++)
            {
                T currentRecord = (T)this.Rows[i].Cells[0].Value;
                if (currentRecord.Equals(record))
                    //http://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
                    if (this.InvokeRequired)
                    {
                        remove<T> d = new remove<T>(removeRecord);
                        this.Invoke(d, new object[] { record });
                    }
                    else
                    {
                        this.Rows.RemoveAt(i);
                    }
                    
            }
        }
    }
}
