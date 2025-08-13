using System.Collections.Generic;

namespace Core.Services.Orders.Models
{
    public sealed class ItemValidationResult
    {
        public List<string> Errors { get; } = new List<string>();

        public List<OrderLineItem> LineItems { get; } = new List<OrderLineItem>();

        public bool Ok => Errors.Count == 0;

        public void AddError(string message)
        {
            if (!string.IsNullOrWhiteSpace(message)) Errors.Add(message);
        }

        public void AddLine(OrderLineItem line) => LineItems.Add(line);
    }
}
