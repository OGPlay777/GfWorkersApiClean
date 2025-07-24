namespace OperationWorker.Core.Models
{
    public class PaintPowder
    {
        private PaintPowder(int id, string paintCode, decimal paintPriceKG)
        {
            Id = id;
            PaintCode = paintCode;
            PaintPriceKG = paintPriceKG;
        }

        public int Id { get; }
        public string PaintCode { get; }
        public decimal PaintPriceKG { get; }

        public static PaintPowder Create(int id, string paintCode, decimal paintPriceKG) =>
            new(id, paintCode, paintPriceKG);
    }
}
