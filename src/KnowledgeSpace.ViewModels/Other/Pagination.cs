using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.ViewModels.Other
{
    public class Pagination<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalRecords { get; set; }
    }
}
