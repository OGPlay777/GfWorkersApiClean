namespace OperationWorker.Core.Models
{
    public class Equipment
    {
        private Equipment(int id, string equipmentName, decimal equipmentSelfcost)
        {
            Id = id;
            EquipmentName = equipmentName;
            EquipmentSelfcost = equipmentSelfcost;
        }

        public int Id { get; }
        public string EquipmentName { get; }
        public decimal EquipmentSelfcost { get; }

        public static Equipment Create(int id, string equipmentName, decimal equipmentSelfcost) =>
            new(id, equipmentName, equipmentSelfcost);
    }
}
