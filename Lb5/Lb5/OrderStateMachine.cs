using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb5
{
    public class OrderStateMachine
    {
        private readonly Dictionary<int, Product> _catalog;

        public OrderStateMachine(Dictionary<int, Product> catalog)
        {
            _catalog = catalog;
        }

        public void PayOrder(OrderContext ctx, decimal paymentAmount)
        {
            Console.WriteLine($"\n> Тригер: PayOrder({paymentAmount:C2}) для Замовлення №{ctx.OrderId} ({ctx.CustomerName})");

            if (ctx.IsFinalState)
            {
                Console.WriteLine($"  [Відхилено] Замовлення перебуває у кінцевому стані {ctx.CurrentState}.");
                return;
            }

            if (ctx.CurrentState != OrderState.Created)
            {
                Console.WriteLine($"  [Відхилено] Оплата можлива лише зі стану Created (поточний: {ctx.CurrentState}).");
                return;
            }

            if (paymentAmount < ctx.TotalAmount)
            {
                Console.WriteLine($"  [Guard False] Недостатньо коштів ({paymentAmount:C2} < {ctx.TotalAmount:C2})! Перехід заблоковано.");
                return;
            }

            foreach (var item in ctx.Items)
            {
                if (!_catalog.TryGetValue(item.ProductId, out var product) || product.StockQuantity < item.Quantity)
                {
                    Console.WriteLine($"  [Guard False] Товару \"{item.ProductName}\" недостатньо на складі! Перехід заблоковано.");
                    return;
                }
            }

            var prevState = ctx.CurrentState;
            ctx.CurrentState = OrderState.Paid;
            Console.WriteLine($"  [Перехід] {prevState} --(PayOrder [Amount >= Total && StockOK]) / провестиОплату()--> {ctx.CurrentState}");

            OnEnterPaid(ctx);
        }

        public void ShipOrder(OrderContext ctx)
        {
            Console.WriteLine($"\n> Тригер: ShipOrder() для Замовлення №{ctx.OrderId} ({ctx.CustomerName})");

            if (ctx.IsFinalState)
            {
                Console.WriteLine($"  [Відхилено] Операція неможлива: замовлення у кінцевому стані {ctx.CurrentState}.");
                return;
            }

            if (ctx.CurrentState != OrderState.Paid)
            {
                Console.WriteLine($"  [Відхилено] Відправка можлива лише зі стану Paid (поточний: {ctx.CurrentState}).");
                return;
            }

            if (!ctx.IsAddressValid)
            {
                Console.WriteLine($"  [Guard False] Не вказано адресу доставки! Перехід заблоковано.");
                return;
            }

            Console.WriteLine("  exit / Закрити фінансову транзакцію та передати накладну на склад");

            var prevState = ctx.CurrentState;
            ctx.CurrentState = OrderState.Shipped;
            Console.WriteLine($"  [Перехід] {prevState} --(ShipOrder [IsAddressValid]) / згенеруватиТТН()--> {ctx.CurrentState}");

            Console.WriteLine($"  entry / Передано службі доставки за адресою: {ctx.DeliveryAddress}");
        }

        public void ConfirmDelivery(OrderContext ctx)
        {
            Console.WriteLine($"\n> Тригер: ConfirmDelivery() для Замовлення №{ctx.OrderId} ({ctx.CustomerName})");

            if (ctx.CurrentState != OrderState.Shipped)
            {
                Console.WriteLine($"  [Відхилено] Підтвердження можливе лише зі стану Shipped (поточний: {ctx.CurrentState}).");
                return;
            }

            var prevState = ctx.CurrentState;
            ctx.CurrentState = OrderState.Completed;
            Console.WriteLine($"  [Перехід] {prevState} --(ConfirmDelivery) / закритиЗамовлення()--> {ctx.CurrentState}");
            Console.WriteLine($"  entry / Нараховано бонусні бали клієнту {ctx.CustomerName}. Замовлення закрито.");
        }

        public void ReturnOrder(OrderContext ctx, int daysSinceDelivery)
        {
            Console.WriteLine($"\n> Тригер: ReturnOrder(days={daysSinceDelivery}) для Замовлення №{ctx.OrderId} ({ctx.CustomerName})");

            if (ctx.CurrentState != OrderState.Shipped)
            {
                Console.WriteLine($"  [Відхилено] Повернення можливе лише зі стану Shipped (поточний: {ctx.CurrentState}).");
                return;
            }

            if (daysSinceDelivery > 14)
            {
                Console.WriteLine($"  [Guard False] Минуло {daysSinceDelivery} дн. (ліміт: <= 14 дн.)! Повернення відхилено.");
                return;
            }

            var prevState = ctx.CurrentState;
            ctx.CurrentState = OrderState.Returned;
            Console.WriteLine($"  [Перехід] {prevState} --(ReturnOrder [days <= 14]) / оформитиПовернення()--> {ctx.CurrentState}");

            RestoreStock(ctx);
            Console.WriteLine($"  entry / Оформлено акт повернення та повернуто {ctx.TotalAmount:C2} клієнту.");
        }

        public void CancelOrder(OrderContext ctx)
        {
            Console.WriteLine($"\n> Тригер: CancelOrder() для Замовлення №{ctx.OrderId} ({ctx.CustomerName})");

            if (ctx.CurrentState is not (OrderState.Created or OrderState.Paid))
            {
                Console.WriteLine($"  [Відхилено] Скасування неможливе зі стану {ctx.CurrentState}.");
                return;
            }

            var prevState = ctx.CurrentState;
            if (prevState == OrderState.Paid)
            {
                Console.WriteLine("  exit / Ініційовано повернення коштів на картку клієнта");
                RestoreStock(ctx);
            }

            ctx.CurrentState = OrderState.Cancelled;
            Console.WriteLine($"  [Перехід] {prevState} --(CancelOrder) / анулюватиЗамовлення()--> {ctx.CurrentState}");
            Console.WriteLine("  entry / Замовлення переведено у статус «Скасовано»");
        }

        private void OnEnterPaid(OrderContext ctx)
        {
            foreach (var item in ctx.Items)
            {
                _catalog[item.ProductId].StockQuantity -= item.Quantity;
            }
            Console.WriteLine($"  entry / Списано товари зі складу та сформовано фіскальний чек на {ctx.TotalAmount:C2}");
        }

        private void RestoreStock(OrderContext ctx)
        {
            foreach (var item in ctx.Items)
            {
                _catalog[item.ProductId].StockQuantity += item.Quantity;
            }
        }
    }
}
