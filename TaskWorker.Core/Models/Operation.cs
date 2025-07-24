namespace OperationWorker.Core.Models
{
    public class Operation
    {
        private Operation(int id, int darbaId, int darbinieks, string darbaVeids, int darbaLaiks, decimal darbaMaksa, string operationType)
        {
            Id = id;
            DarbaID = darbaId;
            Darbinieks = darbinieks;
            DarbaVeids = darbaVeids;
            DarbaLaiks = darbaLaiks;
            DarbaMaksa = darbaMaksa;
            OperationType = operationType;
        }

        public int Id { get; }
        public int DarbaID { get; }
        public int Darbinieks { get; }
        public string DarbaVeids { get; } = string.Empty;
        public int DarbaLaiks { get; }
        public decimal DarbaMaksa { get; } = 0;
        public string OperationType { get; } = string.Empty;

        public static Operation Create(int id, int darbaId, int darbinieks, string darbaVeids, int darbaLaiks, decimal darbaMaksa, string operationType) =>
            new(id, darbaId, darbinieks, darbaVeids, darbaLaiks, darbaMaksa, operationType);
    }
}
