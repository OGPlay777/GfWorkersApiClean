using OperationWorker.Core.Models;
using OperationWorker.DataAccess.Entities;


namespace OperationWorker.DataAccess
{
    public class EntityMapper
    {
        public static GfWorker MapToGfWorker(GfWorkerEntity entity)
        {
            
            var gfWorker = GfWorker.Create(entity.Id, entity.Name, entity.Surname, entity.StundasMaksa, entity.Telefons, entity.Epasts, entity.Komentars, entity.Prasmes);
            return gfWorker;

        }

        public static GfWorkerEntity MapToGfWorkerEntity(GfWorker gfWorker)
        {
            var gfWorkerEntity = new GfWorkerEntity
            {
                Id = gfWorker.Id,
                Name = gfWorker.Name,
                Surname = gfWorker.Surname,
                StundasMaksa = gfWorker.StundasMaksa,
                Telefons = gfWorker.Telefons,
                Epasts = gfWorker.Epasts,
                Komentars = gfWorker.Komentars,
                Prasmes = gfWorker.Prasmes
            };

            return gfWorkerEntity;
        }

        public static Operation MapToOperation(OperationEntity entity)
        {
            var operation = Operation.Create(entity.Id, entity.DarbaID, entity.Darbinieks, entity.DarbaVeids, entity.DarbaLaiks, entity.DarbaMaksa, entity.OperationType);
            return operation;
        }

        public static OperationEntity MapToOperationEntity(Operation operation)
        {
            var opereationEntity = new OperationEntity
            {
                Id = operation.Id,
                DarbaID = operation.DarbaID,
                Darbinieks = operation.Darbinieks,
                DarbaVeids = operation.DarbaVeids,
                DarbaLaiks = operation.DarbaLaiks,
                DarbaMaksa = operation.DarbaMaksa,
                OperationType = operation.OperationType
            };

            return opereationEntity;
        }

        public static Order MapToOrder(OrderEntity entity) 
        {
            var order = Order.Create(
                entity.Id, entity.Pasutitajs, entity.PienemsanasDatums, entity.NodosanasDatums, entity.PasTelefons,
                entity.PasEpasts, entity.Pasutijums, entity.Pienemejs, entity.RekinsKlientam, entity.Pasizmaksa,
                entity.Pelna, entity.Komentars, entity.Operacijas, entity.Image, entity.Status);
            return order;
        }

        public static OrderEntity MapToOrderEntity(Order order)
        {
            var orderEntity = new OrderEntity
            {
                Id = order.Id,
                Pasutitajs = order.Pasutitajs,
                PienemsanasDatums = order.PienemsanasDatums,
                NodosanasDatums = order.NodosanasDatums,
                PasTelefons = order.PasTelefons,
                PasEpasts = order.PasEpasts,
                Pasutijums = order.Pasutijums,
                Pienemejs = order.Pienemejs,
                RekinsKlientam = order.RekinsKlientam,
                Pasizmaksa = order.Pasizmaksa,
                Pelna = order.Pelna,
                Komentars = order.Komentars,
                Operacijas = order.Operacijas,
                Image = order.Image,
                Status = order.Status
            };

            return orderEntity;
        }
        
        public static AppUser MapToUser(AppUserEntity entity)
        {
            var appUser = AppUser.Create(entity.Id, entity.Login, entity.PasswordHash,entity.Telephone, entity.AccessLevel);
            return appUser;
        }

        public static AppUserEntity MapToUserEntity(AppUser user)
        {
            var userEntity = new AppUserEntity
            {
                Id = user.Id,
                Login = user.Login,
                PasswordHash = user.PasswordHash,
                Telephone = user.Telephone,
                AccessLevel = user.AccessLevel
            };

            return userEntity;
        }

        public static Equipment MapToEquipment(EquipmentEntity equipmentEntity)
        {
            var equipment = Equipment.Create(equipmentEntity.Id, equipmentEntity.EquipmentName, equipmentEntity.EquipmentSelfcost);
            return equipment;
        }

        public static EquipmentEntity MapToEquipmentEntity(Equipment equipment)
        {
            var equipmentEntity = new EquipmentEntity
            {
                Id = equipment.Id,
                EquipmentName = equipment.EquipmentName,
                EquipmentSelfcost = equipment.EquipmentSelfcost
            };

            return equipmentEntity;
        }

        public static PaintPowder MapToPaintPowder(PaintPowderEntity paintPowderEntity)
        {
            var paint = PaintPowder.Create(paintPowderEntity.Id, paintPowderEntity.PaintCode, paintPowderEntity.PaintPriceKG);
            return paint;
        }

        public static PaintPowderEntity MapToPaintPowderEntity(PaintPowder paintPowder)
        {
            var paintEntity = new PaintPowderEntity
            {
                Id = paintPowder.Id,
                PaintCode = paintPowder.PaintCode,
                PaintPriceKG = paintPowder.PaintPriceKG
            };

            return paintEntity;
        }
    }
}
