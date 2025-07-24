using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWorker.DataAccess.Entities
{
    public class EquipmentEntity
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public decimal EquipmentSelfcost { get; set; }
    }
}
