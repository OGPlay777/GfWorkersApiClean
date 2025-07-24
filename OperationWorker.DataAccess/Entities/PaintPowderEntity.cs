using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWorker.DataAccess.Entities
{
    public class PaintPowderEntity
    {
        public int Id { get; set; }
        public string PaintCode { get; set; }
        public decimal PaintPriceKG { get; set; }
    }
}
