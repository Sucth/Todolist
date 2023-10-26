using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList_delamort.Model
{
    internal class InitDb
    {
        public static ToDolistContext GetDb()
        {
            using (var db = new ToDolistContext())
            {
                return db;
            }
        }
    }
}
